using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SalesManager.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using SalesManager.Application.Base;
using Microsoft.AspNetCore.Http.Features;
using SalesManager.Application.Base.Services;

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

        public static void AddGenerics(IServiceCollection services, IConfiguration configurations)
        {
            services.AddHttpContextAccessor();
            services.AddAutoMapper(typeof(AssemblyPointer));
            services.AddSingleton<ExceptionMiddleware>();
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddSingleton<ISerializationService, SerializationService>();
            services.AddDateOnlyTimeOnlyStringConverters();
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssemblyContaining<AssemblyPointer>();
            });
        }

        public static void AddFormOptions(IServiceCollection services, IConfiguration configurations)
        {
            services.Configure<FormOptions>(options =>
            {
                options.BufferBody = true;
                options.BufferBodyLengthLimit = long.MaxValue;
                options.KeyLengthLimit = int.MaxValue;
                options.ValueLengthLimit = int.MaxValue;
            });
        }
    }
}
