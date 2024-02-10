using SalesManager.Application.Requests.Admins.Login;
using SalesManager.Application.Requests.Results;

namespace SalesManager.Application.Services
{
    public interface IAdminsService
    {
        Task<AccessTokenResult> LoginAsync(AdminLoginCommand command, CancellationToken cancellationToken);
    }
}
