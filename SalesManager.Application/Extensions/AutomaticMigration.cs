using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SalesManager.Application.Extensions
{
    public static class AutomaticMigration
    {
        public static IApplicationBuilder UseAutomaticMigration<TContext>(this IApplicationBuilder appBuilder)
            where TContext : DbContext
        {
            using var scope = appBuilder.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();

            scope?.ServiceProvider.GetRequiredService<TContext>().Database.Migrate();

            return appBuilder;
        }
    }
}
