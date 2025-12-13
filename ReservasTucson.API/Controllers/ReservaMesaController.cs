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
    public class ReservaMesaController : ControllerBase
    {
        #region Dependencias

        private readonly IReservaMesaService _reservaMesaService;

        #endregion

        #region Constructor

        public ReservaMesaController(IReservaMesaService reservaMesaService)
        {
            _reservaMesaService = reservaMesaService;
        }

        #endregion

        #region Endpoints

        [SwaggerOperation(Summary = "Obtiene todas las reservas con sus mesas")]
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservaMesaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var response = await _reservaMesaService.GetAll();
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }


        [SwaggerOperation(Summary = "Obtiene una reserva especifica")]
        [HttpGet("GetById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservaMesaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var response = await _reservaMesaService.GetById(id);                

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }
        [SwaggerOperation(Summary = "Obtiene las mesas que tiene asignada una reserva")]
        [HttpGet("GetMesasByReservaId/{idReserva}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservaMesaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMesasByReservaId(int idReserva)
        {
            try
            {
                var response = await _reservaMesaService.GetMesasByReservaId(idReserva);                

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        [SwaggerOperation(Summary = "Obtiene una reserva específica")]
        [HttpGet("GetByReservaId/{idReserva}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservaMesaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByReservaId(int idReserva)
        {
            try
            {
                var response = await _reservaMesaService.GetByReservaId(idReserva);
                

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }

        #endregion
    }
}
