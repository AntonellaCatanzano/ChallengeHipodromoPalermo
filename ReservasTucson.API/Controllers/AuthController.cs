using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservasTucson.Domain.DTO.Auth;
using ReservasTucson.Services.Interfaces.Auth;


namespace ReservasTucson.API.Controllers
{
    [ApiController]    
    [Route("api/[controller]")]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDTO)
        {
            var result = await _auth.LoginAsync(loginDTO);

            if (result.Errores != null && result.Errores.Any())
                return Unauthorized(result);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO refreshTokenDTO)
        {
            var result = await _auth.RefreshTokenAsync(refreshTokenDTO);

            if (result.Errores != null && result.Errores.Any())
                return Unauthorized(result);

            return Ok(result);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequestDTO refreshTokenDTO)
        {
            var result = await _auth.LogoutAsync(refreshTokenDTO.RefreshToken);

            if (!result)
                return NotFound(new { mensaje = "El token actual no fue encontrado o ha expirado" });

            return Ok(new { mensaje = "Has cerrado sesión corectamente" });
        }

    }
}

