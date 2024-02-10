namespace SalesManager.Application.Requests.Results
{
    public class RefreshTokenResult
    {
        public RefreshTokenResult()
        {
        }

        public RefreshTokenResult(Guid userId, string tokenString, DateTime expireAt)
        {
            UserId = userId;
            TokenString = tokenString;
            ExpireAt = expireAt;
        }

        public Guid UserId { get; set; }

        public string TokenString { get; set; }

        public DateTime ExpireAt { get; set; }
    }
}
