using ReservasTucson.Authentication.Interfaces;
using ReservasTucson.Domain.DTO.Auth;
using ReservasTucson.Repositories.Interfaces;
using ReservasTucson.Services.Interfaces.Auth;
using System.Security.Cryptography;
using System.Text;

namespace ReservasTucson.Services.Implementations.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly IUsuarioRepository _usuarioRepository;

        public AuthService(ITokenService tokenService, IUsuarioRepository usuarioRepository)
        {
            _tokenService = tokenService;
            _usuarioRepository = usuarioRepository;
        }

        // LOGIN
        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginDTO)
        {
            //await Task.Yield();

            /* var user = _userInMemoryList.FirstOrDefault(u =>
               u.Email.Equals(loginDTO.Email, StringComparison.OrdinalIgnoreCase));

           if (user == null)
               return Error("Usuario o contraseña incorrectos"); */

            var usuario = await _usuarioRepository.GetByEmail(loginDTO.Email);

            if (usuario == null)
                return Error("Usuario o contraseña incorrectos");

            var hashed = ComputeSha256Hash(loginDTO.Password);

            if (usuario.PasswordHash != hashed)
                return Error("Usuario o contraseña incorrectos");

            //if (!user.Activo)
            // return Error("El Usuario está inactivo"); 
            // var response = BuildUserResponse(user);

            var response = new LoginResponseDTO
            {
                Id = usuario.IdUsuario,
                Email = usuario.Email,
                Usuario = usuario.Nombre,
                Activo = true,
                Grupos = new List<string>
                {
                    usuario.TipoUsuario?.Descripcion ?? "Usuario"
                }
            };

            // Generación de tokens
            response = _tokenService.GenerateTokens(response);

            // Persiste refresh token (token actualizado)
            //usuar.refreshToken = response.RefreshToken;
            //user.RefreshTokenExpiration = response.RefreshTokenExpiration;

            response.Mensaje = "Has iniciado sesión correctamente";

            return response;

        }


        // LOGOUT
        // public async Task<bool> LogoutAsync(string refreshToken)
        public async Task<LoginResponseDTO> LogoutAsync()
        {
            await Task.Yield();

            return new LoginResponseDTO
            {
                Mensaje = "Cerraste sesión correctamente"
            };

            // var user = _userInMemoryList.FirstOrDefault(u =>
            // u.RefreshToken == refreshToken);

            // if (user == null)            

            // Invalida el refresh token
            //user.RefreshToken = null;
            // user.RefreshTokenExpiration = null;

        }

        // Manejo de Errores
        private LoginResponseDTO Error(string msg)
        {
            return new LoginResponseDTO
            {
                Errores = new List<string> { msg }
            };
        }

        // Hash de contraseñas
        private static string ComputeSha256Hash(string pass)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(pass));

            var sb = new StringBuilder();

            // Recorre cada byte y se convierte en codigo hexadecimal
            foreach (var b in bytes)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }

        // Creo usuario almacenado en memoria
        /* private class InMemoryUser
         {
             public int Id { get; set; }
             public string Email { get; set; } = null!;
             public string Usuario { get; set; } = null!;
             public string PasswordHash { get; set; } = null!;
             public bool Activo { get; set; }
             public List<string> Grupos { get; set; } = new();
             public string? RefreshToken { get; set; }
             public DateTime? RefreshTokenExpiration { get; set; }
         } */

        // Lista de usuarios simulado que se almacena en memoria 
        /*private static readonly List<InMemoryUser> _userInMemoryList = new()
        {
            new InMemoryUser
            {
                Id = 3,
                Email = "recepcion@tucson.com",
                Usuario = "recepcion",
                PasswordHash = ComputeSha256Hash("recepcionTucson@2025"),
                Activo = true,
                Grupos = new() { "Recepcionista" }
            },
            new InMemoryUser
            {
                Id = 4,
                Email = "gerencia@tucson.com",
                Usuario = "gerencia",
                PasswordHash = ComputeSha256Hash("gerenciaTucson@2025"),
                Activo = true,
                Grupos = new() { "Gerente" }
            }
        }; */



        // Construye la respuesta del login del usuario
        /*private LoginResponseDTO BuildUserResponse(InMemoryUser user)
        {
        return new LoginResponseDTO
        {
            Id = user.Id,
            Email = user.Email,
            Usuario = user.Usuario,
            Activo = user.Activo,
            Grupos = user.Grupos.ToList(),                
            Mensaje = null
        };
        } */


    }
}

