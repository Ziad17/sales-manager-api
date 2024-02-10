namespace SalesManager.Application
{
    public interface ICurrentUserService
    {
        Guid UserId();

        Guid UserId(Guid defaultValue);

        string UserName();

        string UserName(string defaultValue);

        string Name();

        string Phone();

        string Email();

        string Actor();

        string PasswordResetToken();

        string AccessToken();

        string GetClaim(string claimName);

        bool Is(string actor);
    }
}
