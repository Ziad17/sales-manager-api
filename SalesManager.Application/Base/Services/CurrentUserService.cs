using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;

namespace SalesManager.Application.Base.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId()
        {
            if (IsAuthenticated())
                return Guid.Parse(_httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value ?? string.Empty);
            throw new UnauthorizedAccessException("no valid user claims found");
        }

        public Guid UserId(Guid defaultValue)
        {
            if (IsAuthenticated())
                return Guid.Parse(_httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value ?? string.Empty);
            return defaultValue;
        }

        public string UserName()
        {
            if (IsAuthenticated())
                return _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            throw new UnauthorizedAccessException("no valid user claims found");
        }

        public string UserName(string defaultValue)
        {
            if (IsAuthenticated())
                return _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            return defaultValue;
        }

        public string Name()
        {
            if (IsAuthenticated())
                return _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? string.Empty;
            throw new UnauthorizedAccessException("no valid user claims found");
        }

        public string Phone()
        {
            if (IsAuthenticated())
                return _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value ?? string.Empty;
            throw new UnauthorizedAccessException("no valid user claims found");
        }

        public string Email()
        {
            if (IsAuthenticated())
                return _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? string.Empty;
            throw new UnauthorizedAccessException("no valid user claims found");
        }

        public string Actor()
        {
            if (IsAuthenticated())
                return _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Actor)?.Value ?? string.Empty;
            throw new UnauthorizedAccessException("no valid user claims found");
        }

        public string PasswordResetToken()
        {
            if (IsAuthenticated())
                return _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Hash)?.Value ?? string.Empty;
            throw new UnauthorizedAccessException("no valid user claims found");
        }

        public string GetClaim(string claimName)
        {
            if (IsAuthenticated())
                return _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == claimName)?.Value ?? string.Empty;
            return string.Empty;
        }

        public string AccessToken()
        {
            var isAuthorized = _httpContextAccessor.HttpContext!.Request.Headers.TryGetValue(HeaderNames.Authorization, out var authorization);

            if (!isAuthorized)
                throw new InvalidArgumentException(message: "authorization header must be supplied");

            return authorization.ToString()[7..];
        }

        public bool Is(string actor)
        {
            var claim = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Actor)?.Value.ToString();

            if (claim is null)
                throw new InvalidArgumentException(message: "no actor found in claims");

            return string.Equals(claim, actor, StringComparison.CurrentCultureIgnoreCase);
        }

        private bool IsAuthenticated()
        {
            return _httpContextAccessor.HttpContext?.User.Identity is { IsAuthenticated: true };
        }
    }
}
}
