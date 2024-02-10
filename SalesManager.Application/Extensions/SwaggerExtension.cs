using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SalesManager.Application.Configurations;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace SalesManager.Application.Extensions
{
    public static class SwaggerExtension
    {
        /// <summary>
        /// the service layer of swagger implementation
        /// </summary>
        /// <param name="services"></param>
        /// <param name="swaggerConfigurationSectionName">it's optional with a default value called 'Swagger'
        /// ,then it will load the default setting form appsettings.json</param>
        public static void AddSwagger(this IServiceCollection services,
            string swaggerConfigurationSectionName = "Swagger")
        {

            var serviceProvider = services.BuildServiceProvider();
            var configurations = serviceProvider.GetService<IConfiguration>();
            var swaggerConfiguration =
                configurations!.GetSection(swaggerConfigurationSectionName).Get<SwaggerConfiguration>();

            if (swaggerConfiguration!.Enabled)
            {
                services.AddSwaggerGen(options =>
                {

                    var documentFiles = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory),
                        "*.api.xml", SearchOption.AllDirectories);
                    documentFiles.ToList().ForEach(file => options.IncludeXmlComments(file));

                    options.UseInlineDefinitionsForEnums();
                    options.UseDateOnlyTimeOnlyStringConverters();

                    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                    {
                        Description = "Jwt Authorization Header Using Bearer Scheme.\n" +
                                      "Enter 'Bearer' [space] and then your token in the text input below\n" +
                                      "Example: \"Bearer 1231861686016816518a6w1d68as1d86as4d\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = JwtBearerDefaults.AuthenticationScheme
                    });

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = JwtBearerDefaults.AuthenticationScheme
                                },
                                Scheme = "oauth2",
                                Name = JwtBearerDefaults.AuthenticationScheme,
                                In = ParameterLocation.Header
                            },
                            new List<string>()
                        }
                    });
                });
            }
        }

        /// <summary>
        /// the custom swagger middleware
        /// </summary>
        /// <param name="app"></param>
        /// <param name="swaggerConfigurationSectionName">it's optional with a default value called 'Swagger'
        /// ,then it will load the default setting form appsettings.json</param>
        public static void UseSwaggerTool(this IApplicationBuilder app,
            string swaggerConfigurationSectionName = "Swagger")
        {
            var serviceProvider = app.ApplicationServices;
            var configurations = serviceProvider.GetService<IConfiguration>();
            var swaggerConfiguration =
                configurations!.GetSection(swaggerConfigurationSectionName).Get<SwaggerConfiguration>();

            if (!swaggerConfiguration!.Enabled)
                return;

            app.UseSwagger();

            app.UseSwaggerUI(option =>
            {
                option.DocExpansion(DocExpansion.None);
                option.DefaultModelsExpandDepth(-1);
                option.EnableDeepLinking();
                option.DocumentTitle = swaggerConfiguration.Title;
            });
        }
    }
}
