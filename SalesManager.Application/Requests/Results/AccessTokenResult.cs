namespace SalesManager.Application.Requests.Results
{
    public class AccessTokenResult
    {
        public AccessTokenResult()
        {
        }

        public AccessTokenResult(string accessToken, RefreshTokenResult refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public string AccessToken { get; set; }

        public RefreshTokenResult RefreshToken { get; set; }
    }
}
