namespace SalesManager.Application.Configurations
{
    public class UserCreationConfiguration
    {
        public bool RequireUniqueEmail { get; set; }

        public string AdminPassword { get; set; }
    }
}
