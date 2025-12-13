using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ReservasTucson.Authentication.Base;
using ReservasTucson.Authentication.Interfaces;
using ReservasTucson.Authentication.Support.Helpers;
using ReservasTucson.Domain.DTO.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace ReservasTucson.Authentication.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly JwtConfig _jwtConfig;
        private readonly IHttpContextAccessor _context;
        private readonly IJwtTokenGenerator _generator;

        public TokenService(IHttpContextAccessor context, IOptionsMonitor<JwtConfig> options, IJwtTokenGenerator generator)
        {
            _jwtConfig = options.CurrentValue;
            _context = context;
            _generator = generator;
        }

        public LoginResponseDTO GenerateTokens(LoginResponseDTO user)
        {
            // Armo payload mínimo a partir del LoginResponseDTO ya poblado por AuthService
            var payload = new TokenPayload
            {
                Id = user.Id.ToString(),
                Email = user.Email,
                Roles = user.Grupos ?? Enumerable.Empty<string>(), // si usás Grupos como roles, mapear según corresponda
                Grupos = user.Grupos ?? Enumerable.Empty<string>()
            };

            var access = _generator.GenerateAccessToken(payload);
            var refresh = _generator.GenerateRefreshToken();

            user.AccessToken = access.Token;
            user.AccessTokenExpiration = access.Expiration;

            user.RefreshToken = refresh.RefreshToken;
            user.RefreshTokenExpiration = refresh.RefreshTokenExpiration;

            return user;
        }

        public TokenPayload? DecodeCurrentToken()
        {
            var headerToken = _context.HttpContext?
                .Request.Headers["Authorization"]
                .FirstOrDefault()?
                .Split(" ")
                .Last();

            if (string.IsNullOrEmpty(headerToken))
                return null;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_jwtConfig.Secret);

                tokenHandler.ValidateToken(headerToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    ValidateIssuer = true,
                    ValidIssuer = _jwtConfig.Issuer,

                    ValidateAudience = true,
                    ValidAudience = _jwtConfig.Audience,

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwt = (JwtSecurityToken)validatedToken;
                var payloadJson = jwt.Payload.SerializeToJson();
                var payload = JsonSerializer.Deserialize<TokenPayload>(payloadJson);    
                return payload;
            }
            catch
            {
                return null;
            }
        }
    }
}

