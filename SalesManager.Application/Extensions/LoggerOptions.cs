using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Filters;
using Serilog.Settings.Configuration;

namespace SalesManager.Application.Extensions
{
    public static class LoggerOptions
    {
        public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpContextAccessor();

            return (context, configuration) =>
            {
                const string outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}";
                var env = context.HostingEnvironment;
                var seqUrl = context.Configuration.GetConnectionString("seq");


                configuration
                    .ReadFrom.Configuration(context.Configuration, new ConfigurationReaderOptions
                    {
                        SectionName = "Logging"
                    })
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("ApplicationName", env.ApplicationName)
                    .Enrich.WithProperty("EnvironmentName", env.EnvironmentName)
                    .Enrich.WithDemystifiedStackTraces()
                    .WriteTo.Console(outputTemplate: outputTemplate)
                    .Filter.ByExcluding(Matching.WithProperty<string>("SourceContext", p => p == "ZiggyCreatures.Caching.Fusion.FusionCache"))
                    .Filter.ByExcluding(Matching.WithProperty<string>("SourceContext", p => p == "Vizage.Modules.Analytics.Api.Services.HealthChecksPublisherService"))
                    .Filter.ByExcluding(Matching.WithProperty<string>("CacheKey", p => p == "GOOGLE_API_HEALTH"))
                    .Filter.ByExcluding(Matching.WithProperty<string>("HealthCheckName", _ => true));

                if (!string.IsNullOrEmpty(seqUrl))
                {
                    configuration.WriteTo.Seq(seqUrl!);
                }
            };
        }
    }

}
