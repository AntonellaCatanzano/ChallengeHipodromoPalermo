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
    public class EventoController : ControllerBase
    {
        #region Dependencias

        private readonly IEventoService _eventoService;

        #endregion

        #region Constructor

        public EventoController(IEventoService eventoService)
        {
            _eventoService = eventoService;
        }

        #endregion

        #region Endpoints

        [SwaggerOperation(Summary = "Obtiene todos los Eventos para reservar")]
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventoDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var response = await _eventoService.GetAll();                

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }


        [SwaggerOperation(Summary = "Obtiene un evento específico para la reserva")]
        [HttpGet("GetById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventoDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var response = await _eventoService.GetById(id);
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        [SwaggerOperation(Summary = "Inserta un nuevo Evento para agregar a la reserva")]
        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventoDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> InsertEvento([FromBody] EventoDTO model)
        {
            try
            {
                var response = await _eventoService.InsertEvento(model);

                if (response is null)
                {
                    return NotFound();
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Conflict($"No se puede crear el Evento porque {ex.Message}");
            }
        }

        #endregion
    }
}
