namespace ReservasTucson.Domain.DTO.Auth
{
    public class LoginResponseDTO
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Usuario { get; set; } = null!;

        public string AccessToken { get; set; } = null!;
        public DateTime AccessTokenExpiration { get; set; }

        public string RefreshToken { get; set; } = null!;
        public DateTime RefreshTokenExpiration { get; set; }

        public bool Activo { get; set; }
        public List<string> Grupos { get; set; } = new List<string>();

        public List<string> Errores { get; set; } = new List<string>();
        
        public string Mensaje { get; set; } = null!;
    }
}
