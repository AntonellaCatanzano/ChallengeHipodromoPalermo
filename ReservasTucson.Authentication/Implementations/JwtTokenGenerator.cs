using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ReservasTucson.Authentication.Base;
using ReservasTucson.Authentication.Interfaces;
using ReservasTucson.Authentication.Support.Helpers;
using ReservasTucson.Domain.DTO.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ReservasTucson.Authentication.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtConfig _config;

        public JwtTokenGenerator(IOptions<JwtConfig> config)
        {
            _config = config.Value;
        }

        public TokenDTO GenerateAccessToken(TokenPayload payload)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, payload.Id),
                new Claim(JwtRegisteredClaimNames.Email, payload.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            claims.AddRange(payload.Roles.Select(r => new Claim(ClaimTypes.Role, r)));
            claims.AddRange(payload.Grupos.Select(g => new Claim("Grupo", g)));

            var expiration = DateTime.UtcNow.AddMinutes(_config.AccessTokenExpiration);

            var token = new JwtSecurityToken(
                issuer: _config.Issuer,
                audience: _config.Audience,
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new TokenDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

        public RefreshTokenDTO GenerateRefreshToken()
        {
            // Refresh token seguro (64 bytes aleatorios)
            var bytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(bytes);
            var expiration = DateTime.UtcNow.AddDays(_config.RefreshTokenExpiration);

            return new RefreshTokenDTO
            {
                RefreshToken = refreshToken,
                RefreshTokenExpiration = expiration
            };
        }
    }
}



