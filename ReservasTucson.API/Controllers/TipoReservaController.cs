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
    public class TipoReservaController : ControllerBase
    {
        #region Dependencias

        private readonly ITipoReservaService _tipoReservaService;

        #endregion

        #region Constructor

        public TipoReservaController(ITipoReservaService tipoReservaService)
        {
            _tipoReservaService = tipoReservaService;
        }

        #endregion

        #region Endpoints

        [SwaggerOperation(Summary = "Obtiene todos los tipos de reserva que existen")]
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TipoReservaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var response = await _tipoReservaService.GetAll();                

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }


        [SwaggerOperation(Summary = "Obtiene un tipo de reserva específica")]
        [HttpGet("GetById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TipoReservaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var response = await _tipoReservaService.GetById(id);                

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        [SwaggerOperation(Summary = "Inserta un nuevo Tipo de Reserva")]
        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TipoReservaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> InsertUsuario([FromBody] TipoReservaDTO model)
        {
            try
            {
                var response = await _tipoReservaService.InsertTipoReserva(model);                

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Conflict($"No se puede crear el tipo de reserva porque {ex.Message}");
            }
        }

        #endregion
    }
}
