    namespace ReservasTucson.Domain.DTO.Auth
    {
        public class TokenDTO
        {
            public string Token { get; set; } = null!;
            public DateTime Expiration { get; set; }
        }
    }
