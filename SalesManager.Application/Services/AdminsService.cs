using Microsoft.AspNetCore.Identity;
using SalesManager.Application.Requests.Admins.Login;
using SalesManager.Application.Requests.Results;
using SalesManager.Domain.Entities;
using System.Security.Claims;
using SalesManager.Application.Base;
using SalesManager.Application.Base.Services;
using SalesManager.Domain.Exceptions;

namespace SalesManager.Application.Services
{
    public class AdminsService : BaseService, IAdminsService
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public AdminsService(UserManager<User> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<AccessTokenResult> LoginAsync(AdminLoginCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(command.UserName);

            CheckFound(user, "0001");

            if (user is not Admin admin)
                throw new DomainException("unauthorized user access", "0002");

            var userState = await _userManager.CheckPasswordAsync(admin, command.Password);

            if (!userState)
                throw new DomainException("username or password is incorrect.", "0003");

            var roles = await _userManager.GetRolesAsync(admin);

            var claims = GenerateAdminClaims(admin, roles);

            var tokenResult = await _tokenService.GenerateTokensAsync(admin.Id, claims);
            return tokenResult;
        }


        private static Claim[] GenerateAdminClaims(Admin admin, IList<string> roles)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Sid, admin.Id.ToString()),
                new Claim(ClaimTypes.Actor, nameof(admin)),
                new Claim(ClaimTypes.NameIdentifier, admin!.UserName!),
                new Claim(ClaimTypes.Name, admin.Fullname)
            };

            if (roles != null && roles.Any())
            {
                roles = roles.Distinct().ToList();

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            return claims.ToArray();
        }
    }
}
