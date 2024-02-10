using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SalesManager.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using SalesManager.Application.Base;
using Microsoft.AspNetCore.Http.Features;
using SalesManager.Application.Base.Services;
using SalesManager.Plugins.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SalesManager.Application.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SalesManager.Domain.Entities;

namespace SalesManager.Application
{
    public static class DependencyInjection
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configurations)
        {
            var connectionString = configurations.GetConnectionString("Default");

            services.AddDbContext<DatabaseContext>((_, options) =>
            {
                options.UseNpgsql(connectionString!, builder =>
                {
                    builder.EnableRetryOnFailure(5);
                    builder.ExecutionStrategy(dependencies => new NpgsqlRetryingExecutionStrategy(dependencies, 3));
                });
            });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }

        public static void AddGenerics(this IServiceCollection services, IConfiguration configurations)
        {
            services.AddHttpContextAccessor();
            services.AddAutoMapper(typeof(AssemblyPointer));
            services.AddSingleton<ExceptionMiddleware>();
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddValidatorsFromAssemblyContaining<AssemblyPointer>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddSingleton<ISerializationService, SerializationService>();
            services.AddDateOnlyTimeOnlyStringConverters();
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssemblyContaining<AssemblyPointer>();
            });
        }

        public static void AddFormOptions(this IServiceCollection services, IConfiguration configurations)
        {
            services.Configure<FormOptions>(options =>
            {
                options.BufferBody = true;
                options.BufferBodyLengthLimit = long.MaxValue;
                options.KeyLengthLimit = int.MaxValue;
                options.ValueLengthLimit = int.MaxValue;
            });
        }

        public static void AddAuthentication(this IServiceCollection services, IConfiguration configurations)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>()
                                ?? throw new ArgumentNullException(nameof(IConfiguration));

            var authenticationConfigurations = new JwtConfiguration();
            configuration.Bind("Authentications:JwtConfiguration", authenticationConfigurations);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.Audience = authenticationConfigurations.Audience;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = authenticationConfigurations.ValidateIssuerSigningKey,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            System.Text.Encoding.ASCII.GetBytes(authenticationConfigurations.Secret)),
                    ValidateLifetime = authenticationConfigurations.ValidateLifeTime,
                    ValidateAudience = authenticationConfigurations.ValidateIssuer,
                    ValidateIssuer = authenticationConfigurations.ValidateIssuer,
                    ClockSkew = TimeSpan.FromDays(authenticationConfigurations.ClockSkew),
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            services.AddIdentity<User, Role>()
                .AddRoles<Role>()
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddUserStore<UserStore<User, Role, DatabaseContext, Guid, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>>()
                .AddDefaultTokenProviders();


            services.AddAuthorization(options =>
            {
                //TODO:: add policies
            });
        }

        public static void AddIdentityConfigurations(this IServiceCollection services, IConfiguration configurations)
        {
            var jwtConfiguration = new JwtConfiguration();
            configurations!.Bind("Authentications:JwtConfiguration", jwtConfiguration);
            services.AddSingleton(jwtConfiguration);

            var passwordConfiguration = new PasswordPolicyConfiguration();
            configurations.GetSection("Authentications:PasswordPolicy").Bind(passwordConfiguration);
            services.AddSingleton(passwordConfiguration);

            var loginConfiguration = new LoginPolicyConfiguration();
            configurations.GetSection("Authentications:LoginPolicy").Bind(passwordConfiguration);
            services.AddSingleton(loginConfiguration);

            var userCreationConfiguration = new UserCreationConfiguration();
            configurations.GetSection("Authentications:UserCreationPolicy").Bind(userCreationConfiguration);
            services.AddSingleton(userCreationConfiguration);

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = passwordConfiguration.RequiredDigit;
                options.Password.RequireLowercase = passwordConfiguration.RequireLowercase;
                options.Password.RequireNonAlphanumeric = passwordConfiguration.RequireNonAlphanumeric;
                options.Password.RequireUppercase = passwordConfiguration.RequireUppercase;
                options.Password.RequiredLength = passwordConfiguration.RequiredLength;
                options.Password.RequiredUniqueChars = passwordConfiguration.RequiredUniqueChars;

                options.SignIn.RequireConfirmedAccount = loginConfiguration.RequireConfirmedAccount;
                options.SignIn.RequireConfirmedEmail = loginConfiguration.RequireConfirmedEmail;
                options.SignIn.RequireConfirmedPhoneNumber = loginConfiguration.RequireConfirmedPhoneNumber;

                options.User.RequireUniqueEmail = userCreationConfiguration.RequireUniqueEmail;
            });
        }


        public static void AddPlugins(IServiceCollection services, IConfiguration configurations)
        {
            services.AddStorage(configurations);
        }
    }
}
