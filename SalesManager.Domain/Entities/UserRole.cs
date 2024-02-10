using Microsoft.AspNetCore.Identity;

namespace SalesManager.Domain.Entities;

public sealed class UserRole : IdentityUserRole<Guid>
{
    public UserRole(Guid roleId, Guid userId)
    {
        Id = Guid.NewGuid();
        RoleId = roleId;
        UserId = userId;
    }

    public UserRole()
    {
    }

    public Guid Id { get; private set; }

    public Role Role { get; set; }
}
