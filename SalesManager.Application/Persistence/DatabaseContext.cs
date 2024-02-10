using SalesManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace SalesManager.Application.Persistence
{
    public class DatabaseContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public DatabaseContext(DbContextOptions<BaseContext> options)
            : base(options)
        {
        }

        public DatabaseContext(DbContextOptions<BaseContext> options, IServiceProvider serviceProvider)
            : base(options, serviceProvider)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("SalesManager");
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
