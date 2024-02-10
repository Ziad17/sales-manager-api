using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SalesManager.Application.Configurations;

namespace SalesManager.Application.Extensions
{
    public static class CorsExtension
    {
        private const string PolicyName = "Vizage";

        /// <summary>
        /// Add cors configurations
        /// </summary>
        /// <param name="services"></param>
        /// <exception cref="Exception"></exception>
        public static void AddCorsSetup(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var corsConfiguration = new CorsConfigurations();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            configuration.Bind("cors", corsConfiguration);

            if (corsConfiguration is null)
                throw new Exception("Couldn't load cors settings configuration");

            services.AddCors(options =>
            {
                options.AddPolicy(PolicyName, builder =>
                {
                    builder.WithOrigins(corsConfiguration.Origins)
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }

        /// <summary>
        /// setup cors origins
        /// </summary>
        /// <param name="app"></param>
        public static void UseCorsSetup(this IApplicationBuilder app)
        {
            app.UseCors(PolicyName);
        }
    }
}
