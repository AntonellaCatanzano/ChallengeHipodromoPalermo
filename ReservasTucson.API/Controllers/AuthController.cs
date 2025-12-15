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

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _auth.LogoutAsync();

            if (!string.IsNullOrEmpty(result.Errores?.FirstOrDefault()))
                return NotFound(new { mensaje = "El token no es válido o ya expiró" });            

            return Ok(new { mensaje = "Has cerrado sesión corectamente" });
        }

    }
}

