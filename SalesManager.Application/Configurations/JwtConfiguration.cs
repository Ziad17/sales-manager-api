namespace SalesManager.Application.Configurations
{
    public class JwtConfiguration
    {
        public string Secret { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string Authority { get; set; }

        public bool ValidateIssuer { get; set; }

        public bool ValidateAudience { get; set; }

        public bool ValidateLifeTime { get; set; }

        public bool ValidateIssuerSigningKey { get; set; }

        public int AccessTokenExpiration { get; set; }

        public int RefreshTokenExpiration { get; set; }

        public int RemoveCachedRefreshTokensEvery { get; set; } = 1;

        public int ClockSkew { get; set; } = 1;
    }
}
