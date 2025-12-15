
using ReservasTucson.Domain.DTO.Auth;
using System.Threading.Tasks;

namespace ReservasTucson.Services.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginDTO);
        // Task<LoginResponseDTO> RefreshTokenAsync(RefreshTokenRequestDTO refreshTokenDTO);
        // Task<bool> LogoutAsync(string refreshTokenDTO);
        Task<LoginResponseDTO> LogoutAsync();
    }
}
