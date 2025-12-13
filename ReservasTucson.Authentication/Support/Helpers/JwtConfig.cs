using System.Text.Json.Serialization;


namespace ReservasTucson.Authentication.Support.Helpers
{
    public class JwtConfig
    {
        [JsonPropertyName("secret")]
        public string Secret { get; set; } = null!;

        [JsonPropertyName("issuer")]
        public string Issuer { get; set; } = null!;

        [JsonPropertyName("audience")]
        public string Audience { get; set; } = null!;

        [JsonPropertyName("accessTokenExpiration")]
        public int AccessTokenExpiration { get; set; }

        [JsonPropertyName("refreshTokenExpiration")]
        public int RefreshTokenExpiration { get; set; }
    }
}
