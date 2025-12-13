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
    public class DetalleReservaController : ControllerBase
    {
        #region Dependencias

        private readonly IDetalleReservaService _detalleReservaService;

        #endregion

        #region Constructor

        public DetalleReservaController(IDetalleReservaService detalleReservaService)
        {
            _detalleReservaService = detalleReservaService;
        }

        #endregion

        #region Endpoints

        [SwaggerOperation(Summary = "Obtiene todos los Reservas con su Detalle")]
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DetalleReservaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var response = await _detalleReservaService.GetAll();
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }


        [SwaggerOperation(Summary = "Obtiene el DetalleReserva por Id")]
        [HttpGet("GetById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DetalleReservaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var response = await _detalleReservaService.GetById(id);                

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }
        

        [SwaggerOperation(Summary = "Obtiene el Detalle según una reserva específica")]
        [HttpGet("GetByReservaId/{idReserva}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DetalleReservaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByReservaId(int idReserva)
        {
            try
            {
                var response = await _detalleReservaService.GetByReservaId(idReserva);                

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
