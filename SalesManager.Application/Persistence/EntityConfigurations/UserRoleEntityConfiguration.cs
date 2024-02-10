using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesManager.Domain.Entities;

namespace SalesManager.Application.Persistence.EntityConfigurations
{
    public class UserRoleEntityConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRoles");

            builder.HasOne(c => c.Role)
                .WithMany(role => role.UserRoles)
                .HasForeignKey(c => c.RoleId)
                .IsRequired();

            builder.HasKey(c => c.Id);
            builder.HasIndex(c => c.RoleId);
            builder.HasIndex(c => c.UserId);
        }
    }
}
