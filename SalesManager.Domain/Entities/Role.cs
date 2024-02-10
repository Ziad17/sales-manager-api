using Microsoft.AspNetCore.Identity;

namespace SalesManager.Domain.Entities
{
    public sealed class Role : IdentityRole<Guid>
    {
        public Role(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        private Role()
        {
        }

        public ICollection<UserRole> UserRoles { get; set; }
    }
}
