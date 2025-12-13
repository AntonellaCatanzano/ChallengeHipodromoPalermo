namespace ReservasTucson.Domain.DTO.Auth
{
    public class RefreshTokenDTO
    {
        public string RefreshToken { get; set; } = null!;
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
