namespace SalesManager.Application.Configurations
{
    public class LoginPolicyConfiguration
    {
        public bool RequireConfirmedAccount { get; set; }

        public bool RequireConfirmedEmail { get; set; }

        public bool RequireConfirmedPhoneNumber { get; set; }
    }
}
