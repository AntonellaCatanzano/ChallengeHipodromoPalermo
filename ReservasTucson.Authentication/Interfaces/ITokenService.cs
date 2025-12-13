using ReservasTucson.Authentication.Base;
using ReservasTucson.Domain.DTO.Auth;

namespace ReservasTucson.Authentication.Interfaces
{
    public interface ITokenService
    {
        /// <summary>
        /// Genera Access + Refresh tokens a partir del LoginResponseDTO (usuario validado).
        /// Devuelve el mismo LoginResponseDTO con AccessToken, RefreshToken y expiraciones pobladas.
        /// </summary>
        LoginResponseDTO GenerateTokens(LoginResponseDTO user);

        /// <summary>
        /// Decodifica el token actual (extraído de Authorization header) y devuelve el payload o null.
        /// </summary>
        TokenPayload? DecodeCurrentToken();

        // string GenerateJwtToken(LoginResponseDTO pars);

        //TokenPayload DecodeToken();
    }
    
}
