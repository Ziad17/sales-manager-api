using MediatR;
using SalesManager.Application.Requests.Results;
using SalesManager.Application.Services;

namespace SalesManager.Application.Requests.Admins.Login
{
    public class AdminLoginCommand : IRequest<AccessTokenResult>
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public class AdminLoginCommandHandler : IRequestHandler<AdminLoginCommand, AccessTokenResult>
    {
        private readonly IAdminsService _adminsService;

        public AdminLoginCommandHandler(IAdminsService adminsService)
        {
            _adminsService = adminsService;
        }
        public async Task<AccessTokenResult> Handle(AdminLoginCommand request, CancellationToken cancellationToken)
        {
            return await _adminsService.LoginAsync(request, cancellationToken);
        }
    }
}
