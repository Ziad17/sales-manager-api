using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalesManager.Application.Base;
using SalesManager.Application.Base.Services;

namespace SalesManager.Application.Persistence
{
    public class DatabaseContext : DbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeService _dateTimeService;

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options, IServiceProvider serviceProvider)
            : base(options)
        {
            _currentUserService = serviceProvider.GetService<ICurrentUserService>();
            _dateTimeService = serviceProvider.GetRequiredService<IDateTimeService>();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (_currentUserService != null)
                Audit();

            return await base.SaveChangesAsync(cancellationToken);
        }

        private void Audit()
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.Entity.SetModifiedById(_currentUserService.UserId(Guid.Empty));
                        entry.Entity.SetModificationDate(_dateTimeService.Now());
                        entry.Entity.SetModifiedByName(_currentUserService.UserName(string.Empty));
                        break;
                    case EntityState.Added:
                        entry.Entity.SetCreatedById(_currentUserService.UserId(Guid.Empty));
                        entry.Entity.SetCreationDate(_dateTimeService.Now());
                        entry.Entity.SetCreatedByName(_currentUserService.UserName(string.Empty));
                        break;
                }
            }
        }
    }
}
