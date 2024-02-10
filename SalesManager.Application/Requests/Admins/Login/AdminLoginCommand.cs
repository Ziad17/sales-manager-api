using MediatR;
using SalesManager.Application.Requests.Results;

namespace SalesManager.Application.Requests.Admins.Login
{
    public class AdminLoginCommand : IRequest<AccessTokenResult>
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
