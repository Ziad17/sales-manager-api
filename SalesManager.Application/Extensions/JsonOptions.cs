using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace SalesManager.Application.Extensions
{
    public static class JsonOptions
    {
        public static IMvcBuilder ConfigureJsonOptions(this IMvcBuilder builder)
        {
            builder.AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            return builder;
        }
    }
}
