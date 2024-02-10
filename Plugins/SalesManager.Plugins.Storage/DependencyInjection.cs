using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vizage.Infrastructure.Storage;

namespace SalesManager.Plugins.Storage
{
    public static class DependencyInjection
    {
        public static void AddStorage(this IServiceCollection services, IConfiguration configurations)
        {
            services.AddScoped<IStorageService, StorageService>();
        }
    }
}
