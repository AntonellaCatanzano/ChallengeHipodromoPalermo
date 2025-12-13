using ReservasTucson.Authentication.Base;
using ReservasTucson.Domain.DTO.Auth;

namespace ReservasTucson.Authentication.Interfaces
{
    public interface IJwtTokenGenerator
    {
        TokenDTO GenerateAccessToken(TokenPayload payload);
        RefreshTokenDTO GenerateRefreshToken();
    }
}

