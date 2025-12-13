using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservasTucson.Domain.DTO;
using ReservasTucson.Domain.Support.Helpers;
using ReservasTucson.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;


namespace ReservasTucson.API.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private readonly IReservaService _reservaService;
        private readonly IUsuarioService _usuarioService;

        public ReservaController(IReservaService reservaService, IUsuarioService usuarioService)
        {
            _reservaService = reservaService;
            _usuarioService = usuarioService;
        }

        

        #region Crear Reservas

        [HttpPost("CrearEstandar")]
        [SwaggerOperation(Summary = "Crea una reserva estándar")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CrearReservaEstandar([FromBody] ReservaCreateStandardDTO reservaStandardDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {

                int usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var result = await _reservaService.CrearReservaEstandarAsync(reservaStandardDto, usuarioId);

                return Ok(result);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("CrearVip")]
        [SwaggerOperation(Summary = "Crea una reserva VIP")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CrearReservaVip([FromBody] ReservaCreateVipDTO reservaVipDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                int usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var result = await _reservaService.CrearReservaVipAsync(reservaVipDto, usuarioId);

                return Ok(result);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("CrearCumple")]
        [SwaggerOperation(Summary = "Crea una reserva de cumpleaños")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CrearReservaCumple([FromBody] ReservaCreateCumpleDTO reservaCumpleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                int usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var result = await _reservaService.CrearReservaCumpleAsync(reservaCumpleDto, usuarioId);

                return Ok(result);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #endregion

        #region Acciones sobre Reserva

        [HttpPost("Confirmar/{reservaId}")]
        [SwaggerOperation(Summary = "Confirma una reserva")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConfirmarReserva(int reservaId)
        {
            try
            {

                var result = await _reservaService.ConfirmarReservaAsync(reservaId);

                return Ok(result);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("Cancelar/{reservaId}")]
        [SwaggerOperation(Summary = "Cancela una reserva")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CancelarReserva(int reservaId, [FromBody] string observacion)
        {
            try
            {
                var result = await _reservaService.CancelarReservaAsync(reservaId, observacion);

                return Ok(result);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("NoAsistio/{reservaId}")]
        [SwaggerOperation(Summary = "Marca que el cliente no asistió a la reserva")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> MarcarNoAsistio(int reservaId)
        {
            try
            {
                var result = await _reservaService.MarcarNoAsistioAsync(reservaId);

                return Ok(result);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #endregion

        #region Consultas

        [HttpGet("GetById/{id}")]
        [SwaggerOperation(Summary = "Obtiene el detalle de una reserva por ID")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservaDetailDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _reservaService.GetByIdAsync(id);
                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetAll")]
        [SwaggerOperation(Summary = "Obtiene todas las reservas con filtros opcionales")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ReservaListItemDTO>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(
            int page = 1, int pageSize = 20,
            DateTime? fecha = null,
            int? tipoReservaId = null,
            int? estadoReservaId = null,
            string? nombre = null,
            string? email = null)
        {
            try
            {
                var result = await _reservaService.GetAllAsync(page, pageSize, fecha, tipoReservaId, estadoReservaId, nombre, email);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("AsignarMesas")]
        [SwaggerOperation(Summary = "Asigna mesas a una reserva")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<int>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AsignarMesas([FromBody] AsignarMesasRequestDTO mesasDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _reservaService.AsignarMesasAsync(mesasDto);

                return Ok(result);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #endregion
    }
}
