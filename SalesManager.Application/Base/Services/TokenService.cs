using Microsoft.IdentityModel.Tokens;
using SalesManager.Application.Configurations;
using SalesManager.Application.Requests.Results;
using SalesManager.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using SalesManager.Application.Persistence;

namespace SalesManager.Application.Base.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtConfiguration _jwtConfiguration;
        private readonly IRepository<User> _usersRepository;
        private readonly IDateTimeService _dateTimeService;
        private readonly byte[] _secret;

        public TokenService(JwtConfiguration jwtConfiguration, IRepository<User> usersRepository, IDateTimeService dateTimeService)
        {
            _jwtConfiguration = jwtConfiguration;
            _usersRepository = usersRepository;
            _dateTimeService = dateTimeService;
            _secret = Encoding.ASCII.GetBytes(jwtConfiguration.Secret);
        }

        public async Task<AccessTokenResult> GenerateTokensAsync(Guid userId, Claim[] claims)
        {
            var shouldAddAudienceClaim = string.IsNullOrWhiteSpace(claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)
                ?.Value);

            var jwtToken = new JwtSecurityToken(
                _jwtConfiguration.Issuer,
                shouldAddAudienceClaim ? _jwtConfiguration.Audience : string.Empty,
                claims,
                expires: _dateTimeService.Now().AddMinutes(_jwtConfiguration.AccessTokenExpiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(_secret),
                    SecurityAlgorithms.HmacSha256Signature));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var refreshTokenResult = new RefreshTokenResult(userId, GenerateRefreshTokenString(), _dateTimeService.Now().AddMinutes(_jwtConfiguration.RefreshTokenExpiration));

            var user = await _usersRepository.GetByIdAsync(userId);

            user!.AssignRefreshToken(refreshTokenResult.TokenString, refreshTokenResult.ExpireAt, _dateTimeService.Now());

            await _usersRepository.UpdateAsync(user);

            return new AccessTokenResult(accessToken, refreshTokenResult);
        }

        public async Task<AccessTokenResult> RefreshAsync(string refreshToken, string accessToken)
        {
            (ClaimsPrincipal principal, JwtSecurityToken jwtToken) = DecodeJwtToken(accessToken);

            var userId = await ValidateTokenAsync(jwtToken, principal, refreshToken);

            return await GenerateTokensAsync(userId, principal.Claims.ToArray());
        }

        public async Task<AccessTokenResult> RefreshAsync(string refreshToken, string accessToken, List<Claim> claims)
        {
            (ClaimsPrincipal principal, JwtSecurityToken jwtToken) = DecodeJwtToken(accessToken);

            var userId = await ValidateTokenAsync(jwtToken, principal, refreshToken);

            var newClaims = principal.Claims.Where(c => c.Type != ClaimTypes.Role).ToList();

            newClaims.AddRange(claims);

            return await GenerateTokensAsync(userId, newClaims.ToArray());
        }

        public (ClaimsPrincipal principal, JwtSecurityToken) DecodeJwtToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new SecurityTokenException("Invalid token");

            var principal = new JwtSecurityTokenHandler()
                .ValidateToken(token,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = _jwtConfiguration.ValidateIssuer,
                        ValidIssuer = _jwtConfiguration.Issuer,
                        ValidateIssuerSigningKey = _jwtConfiguration.ValidateIssuerSigningKey,
                        IssuerSigningKey = new SymmetricSecurityKey(_secret),
                        ValidAudience = _jwtConfiguration.Audience,
                        ValidateAudience = _jwtConfiguration.ValidateAudience,
                        ValidateLifetime = _jwtConfiguration.ValidateLifeTime,
                        ClockSkew = TimeSpan.FromMinutes(_jwtConfiguration.ClockSkew)
                    },
                    out var validatedToken);

            return (principal, validatedToken as JwtSecurityToken);
        }

        private static string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<Guid> ValidateTokenAsync(JwtSecurityToken jwtToken, ClaimsPrincipal principal, string refreshToken)
        {
            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
                throw new SecurityTokenException("Unverified token");

            var userId = Guid.Parse(principal.Claims.FirstOrDefault(d => d.Type == ClaimTypes.Sid)?.Value!);

            var user = await _usersRepository.GetByIdAsync(userId);

            user!.ValidateToken(refreshToken, _dateTimeService.Now());

            return userId;
        }
    }
}
