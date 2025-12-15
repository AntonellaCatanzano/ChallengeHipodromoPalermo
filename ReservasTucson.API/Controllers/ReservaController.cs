using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservasTucson.Domain.DTO;
using ReservasTucson.Domain.Support.Helpers;
using ReservasTucson.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReservasTucson.API.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private readonly IReservaService _reservaService;

        public ReservaController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        #region Crear Reservas
        [HttpPost("CrearEstandar")]
        [SwaggerOperation(Summary = "Crea una reserva estándar")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ReservaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CrearReservaEstandar([FromBody] ReservaCreateStandardDTO reservaStandardDto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            try
            {
                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _reservaService.CrearReservaEstandarAsync(reservaStandardDto, usuarioId);
                return CreatedAtAction(nameof(CrearReservaEstandar), new { id = result.Id }, result);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocurrió un error: {ex.Message}");
            }
        }

        [HttpPost("CrearVip")]
        [SwaggerOperation(Summary = "Crea una reserva VIP")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ReservaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CrearReservaVip([FromBody] ReservaCreateVipDTO reservaVipDto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            try
            {
                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _reservaService.CrearReservaVipAsync(reservaVipDto, usuarioId);
                return CreatedAtAction(nameof(CrearReservaVip), new { id = result.Id }, result);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocurrió un error: {ex.Message}");
            }
        }

        [HttpPost("CrearCumple")]
        [SwaggerOperation(Summary = "Crea una reserva de cumpleaños")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ReservaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CrearReservaCumple([FromBody] ReservaCreateCumpleDTO reservaCumpleDto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            try
            {
                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _reservaService.CrearReservaCumpleAsync(reservaCumpleDto, usuarioId);
                return CreatedAtAction(nameof(CrearReservaCumple), new { id = result.Id }, result);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocurrió un error: {ex.Message}");
            }
        }
        #endregion

        #region Acciones de Reserva

        [Authorize(Roles = "Recepcionista")]
        [HttpPost("{reservaId}/Confirmar")]
        [SwaggerOperation(Summary = "Confirma una reserva")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservaDTO))]
        public async Task<IActionResult> ConfirmarReserva(int reservaId)
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var result = await _reservaService.ConfirmarAsync(reservaId, usuarioId);

                return Ok(result);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "Recepcionista")]
        [HttpPost("Cancelar/{reservaId}")]
        [SwaggerOperation(Summary = "Cancela una reserva")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservaDTO))]
        public async Task<IActionResult> CancelarReserva(int reservaId, [FromBody] string observacion)
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var result = await _reservaService.CancelarAsync(reservaId, observacion, usuarioId);
                return Ok(result);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "Recepcionista")]
        [HttpPost("NoAsistio/{reservaId}")]
        [SwaggerOperation(Summary = "Marca una reserva como No Asistió")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservaDTO))]
        public async Task<IActionResult> MarcarNoAsistio(int reservaId)
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var result = await _reservaService.MarcarNoAsistioAsync(reservaId, usuarioId);

                return Ok(result);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        #endregion

        #region Consultas

        [Authorize(Roles = "Recepcionista")]
        [HttpGet("GetDetalle/{id}")]
        [SwaggerOperation(Summary = "Obtiene el detalle de una reserva por Id")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservaDetailDTO))]
        public async Task<IActionResult> GetDetalleReserva(int id)
        {
            try
            {
                var result = await _reservaService.GetDetalleReservaAsync(id);
                return Ok(result);
            }
            catch (BusinessException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "Recepcionista")]
        [HttpGet("ListadoConFiltros")]
        [SwaggerOperation(Summary = "Obtiene reservas paginadas con filtros")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<ReservaListItemDTO>))]
        public async Task<IActionResult> GetReservasPaginadas([FromQuery] ReservaFilterDTO filtros, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _reservaService.GetReservasPaginadasAsync(filtros, pageNumber, pageSize);
            return Ok(result);
        }

        [Authorize(Roles = "Recepcionista")]
        [HttpGet("GetAll")]
        [SwaggerOperation(Summary = "Obtiene todas las reservas")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ReservaDTO>))]
        public async Task<IActionResult> GetAllReservas()
        {
            var result = await _reservaService.GetAllAsync();
            return Ok(result);
        }

        #endregion
    }
}
