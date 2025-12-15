using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservasTucson.Domain.DTO;
using ReservasTucson.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace ReservasTucson.API.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class MesaController : ControllerBase
    {
        #region Dependencias

        private readonly IMesaService _mesaService;

        #endregion

        #region Constructor

        public MesaController(IMesaService mesaService)
        {
            _mesaService = mesaService;
        }

        #endregion

        #region Endpoints

        [SwaggerOperation(Summary = "Obtiene todas las mesas que se pueden reservar")]
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MesaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var response = await _mesaService.GetAll();
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }


        [SwaggerOperation(Summary = "Obtiene una mesa específica")]
        [HttpGet("GetById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MesaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var response = await _mesaService.GetById(id);
                

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        [SwaggerOperation(Summary = "Inserta una nueva Mesa para la reserva")]
        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MesaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> InsertMesa([FromBody] MesaDTO model)
        {
            try
            {
                var response = await _mesaService.InsertMesa(model);                

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Conflict($"No se puede crear la mesa porque {ex.Message}");
            }
        }

        [SwaggerOperation(Summary = "Verifica si una mesa específica está disponible para una fecha y duración")]
        [HttpGet("EstaDisponible/{mesaId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> EstaDisponible(
            int mesaId,
            [FromQuery] DateTime fechaHora,
            [FromQuery] int duracionMinutos)
        {
            try
            {
                var disponible = await _mesaService.EstaDisponibleAsync(mesaId, fechaHora, duracionMinutos);
                return Ok(disponible);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        

        #endregion
    }
}
