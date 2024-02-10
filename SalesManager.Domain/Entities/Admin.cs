namespace SalesManager.Domain.Entities
{
    public class Admin : User
    {
        private Admin(Guid id, string username, string fullname)
            : base(id, username, fullname)
        {
            IsSuperAdmin = true;
        }

        public Admin()
        {
        }

        public bool IsSuperAdmin { get; set; }

        public static Admin SuperAdmin =>
            new Admin(Guid.Parse("0b51dfed-a2c3-4155-b37b-9ef91ada5239"), "admin", "SuperAdmin");
    }
}
