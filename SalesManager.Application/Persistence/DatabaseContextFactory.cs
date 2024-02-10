using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace SalesManager.Application.Persistence
{
    internal class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<GenericContext>();
            builder.UseNpgsql("Host=localhost;Port=5432;Database=sales-manager;Username=postgres;password=admin;");
            return new DatabaseContext(builder.Options);
        }
    }
}
