using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using SalesManager.Application.Persistence;

namespace SalesManager.Application.Base.Services
{
    public static class DependencyInjection
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configurations)
        {
            var connectionString = configurations.GetConnectionString("Default");


            services.AddDbContext<DatabaseContext>((serviceProviderContext, options) =>
            {
                options.UseNpgsql(connectionString!, builder =>
                {
                    builder.EnableRetryOnFailure(5);
                    builder.ExecutionStrategy(dependencies => new NpgsqlRetryingExecutionStrategy(dependencies, 3));
                });
            });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}
