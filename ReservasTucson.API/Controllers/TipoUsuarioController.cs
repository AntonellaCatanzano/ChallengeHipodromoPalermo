using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservasTucson.Domain.DTO;
using ReservasTucson.Services.Implementations;
using ReservasTucson.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace ReservasTucson.API.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TipoUsuarioController : ControllerBase
    {
        #region Dependencias

        private readonly ITipoUsuarioService _tipoUsuarioService;

        #endregion

        #region Constructor

        public TipoUsuarioController(ITipoUsuarioService tipoUsuarioService)
        {
            _tipoUsuarioService = tipoUsuarioService;
        }

        #endregion

        #region Endpoints

        [SwaggerOperation(Summary = "Obtiene todos los tipos de Usuarios")]
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TipoUsuarioDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var response = await _tipoUsuarioService.GetAll();                

                return Ok(response);
            }
            catch (Exception ex)
            {

                //Log.Error(ex, $"Ocurrió un error en el controlador {ControllerName}, método {MethodName}.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [SwaggerOperation(Summary = "Obtiene un tipo de usuario específico")]
        [HttpGet("GetById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TipoUsuarioDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var response = await _tipoUsuarioService.GetById(id);                

                return Ok(response);
            }
            catch (Exception ex)
            {

                //Log.Error(ex, $"Ocurrió un error en el controlador {ControllerName}, método {MethodName}.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [SwaggerOperation(Summary = "Inserta un nuevo tipo de Usuario")]
        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TipoUsuarioDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> InsertUsuario([FromBody] TipoUsuarioDTO model)
        {
            try
            {
                var response = await _tipoUsuarioService.InsertTipoUsuario(model);                

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Conflict($"No se puede crear el tipo de Usuario porque {ex.Message}");
            }
        }

        #endregion
    }
}
