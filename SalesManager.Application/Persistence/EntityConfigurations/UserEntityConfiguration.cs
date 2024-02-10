using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SalesManager.Domain.Entities;

namespace SalesManager.Application.Persistence.EntityConfigurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.OwnsOne(c => c.RefreshToken, navigationBuilder =>
            {
                navigationBuilder.ToTable("Users.RefreshTokens");
                navigationBuilder.WithOwner().HasForeignKey("UserId");
                navigationBuilder.Property<Guid>("Id");
                navigationBuilder.HasKey("Id");
            });

            builder.Property(c => c.IsDeleted)
                .HasDefaultValue(false);

            builder.HasQueryFilter(user => !user.IsDeleted);
        }
    }
}
