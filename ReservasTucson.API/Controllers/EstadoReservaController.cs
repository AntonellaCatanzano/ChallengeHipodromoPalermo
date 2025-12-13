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
    public class EstadoReservaController : ControllerBase
    {
        #region Dependencias

        private readonly IEstadoReservaService _estadoReservaService;

        #endregion

        #region Constructor

        public EstadoReservaController(IEstadoReservaService estadoReservaService)
        {
            _estadoReservaService = estadoReservaService;
        }

        #endregion

        #region Endpoints

        [SwaggerOperation(Summary = "Obtiene todos los Estados de las Reservas")]
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EstadoReservaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var response = await _estadoReservaService.GetAll();

                return Ok(response); 
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [SwaggerOperation(Summary = "Obtiene el estado de reserva específica")]
        [HttpGet("GetById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EstadoReservaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var response = await _estadoReservaService.GetById(id);                

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        [SwaggerOperation(Summary = "Inserta un nuevo Estado para la Reserva")]
        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EstadoReservaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> InsertUsuario([FromBody] EstadoReservaDTO model)
        {
            try
            {
                var response = await _estadoReservaService.InsertEstadoReserva(model);                

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Conflict($"No se puede crear el Estado para la Reserva porque {ex.Message}");
            }
        }        

        #endregion
    }
}
