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

        public static Role Driver => new Role(Guid.Parse("a55eef24-3edb-41a0-8329-1462dee9aeb9"), "Driver");
        public static Role Admin => new Role(Guid.Parse("60091d58-d6bd-471e-8d7a-5fb7afd84385"), "Admin");
    }
}
