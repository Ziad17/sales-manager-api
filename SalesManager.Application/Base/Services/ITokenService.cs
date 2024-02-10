using SalesManager.Application.Requests.Results;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SalesManager.Application.Base.Services
{
    public interface ITokenService
    {
        Task<AccessTokenResult> GenerateTokensAsync(Guid userId, Claim[] claims);

        Task<AccessTokenResult> RefreshAsync(string refreshToken, string accessToken);

        Task<AccessTokenResult> RefreshAsync(string refreshToken, string accessToken, List<Claim> claims);

        (ClaimsPrincipal principal, JwtSecurityToken) DecodeJwtToken(string token);
    }
}
