using Ardalis.Specification;
using Microsoft.AspNetCore.Identity;
using SalesManager.Domain.Exceptions;

namespace SalesManager.Domain.Entities
{
    public class User : IdentityUser<Guid>, IEntity<Guid>
    {
        protected User()
        {
        }

        public string Fullname { get; set; }

        public string Avatar { get; set; }

        public bool IsVerified { get; set; }

        public bool IsSuspend { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreationDate { get; set; }

        public RefreshToken RefreshToken { get; set; }

        public DateTime UpdatedOn { get; set; }

        public DateTime LastLogin { get; set; }


        public void AssignRefreshToken(string tokenString, DateTime expireAt, DateTime now)
        {
            if (RefreshToken is null)
            {
                RefreshToken = new RefreshToken(tokenString, expireAt, now);
                return;
            }

            RefreshToken.SetCreatedAt(now);
            RefreshToken.SetExpireAt(expireAt);
            RefreshToken.SetValue(tokenString);
        }

        public void ValidateToken(string refreshToken, DateTime now)
        {
            if (RefreshToken == null)
                throw new DomainException("refresh token not found");

            if (RefreshToken.Value != refreshToken || RefreshToken.ExpireAt < now)
                throw new DomainException("Expired Token");
        }
    }
}
