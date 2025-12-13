using AutoMapper;
using ReservasTucson.Domain.DTO;
using ReservasTucson.Domain.Entities;
using ReservasTucson.Domain.Enums;
using ReservasTucson.Domain.Support.Helpers;
using ReservasTucson.Repositories.Interfaces.UoW;
using ReservasTucson.Services.Interfaces;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;

namespace ReservasTucson.Services.Implementations
{
    public class ReservaService : IReservaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReservaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        
        public async Task<ReservaDTO> CrearReservaEstandarAsync(ReservaCreateStandardDTO reservaEstandarDto, int usuarioId)
        {
            var fechaHoraUtc = ParseFechaHora(reservaEstandarDto.FechaHora);

            if (fechaHoraUtc <= DateTime.UtcNow)
                throw new BusinessException("No se permiten reservas pasadas.");

            var horaLocal = fechaHoraUtc.ToLocalTime().TimeOfDay;

            if (!(horaLocal >= new TimeSpan(12, 0, 0) || horaLocal <= new TimeSpan(1, 0, 0)))
                throw new BusinessException("Horario Estandar: horario 19:00–23:30");

            if (reservaEstandarDto.CantidadPersonas < 1)
                throw new BusinessException("La cantidad mínima es entre 1 y 4 personas.");
            
            

            var cliente = await ObtenerOCrearClienteAsync(
                reservaEstandarDto.IdCliente, reservaEstandarDto.Nombre, reservaEstandarDto.Apellido, reservaEstandarDto.Email, reservaEstandarDto.Telefono, reservaEstandarDto.Cuit);

           
            
            var entity = _mapper.Map<Reserva>(reservaEstandarDto);

            entity.FechaHora = fechaHoraUtc;
            entity.ClienteId = cliente.Id;
            entity.UsuarioId = usuarioId; 
            entity.TipoReservaId = (int)TipoReservaEnum.Estandar;
            entity.EstadoReservaId = (int)EstadoReservaEnum.Pendiente;
            entity.SeniaPagada = false;
           
            entity.DetalleReserva = new DetalleReserva();

            var created = await _unitOfWork.ReservaRepository.CrearReservaEstandarAsync(entity);

            await _unitOfWork.SaveChanges();

            return _mapper.Map<ReservaDTO>(created);
        }

        
        public async Task<ReservaDTO> CrearReservaVipAsync(ReservaCreateVipDTO reservaVipDto, int usuarioId)
        {
            var fechaHoraUtc = ParseFechaHora(reservaVipDto.FechaHora);

            if (fechaHoraUtc <= DateTime.UtcNow)
                throw new BusinessException("No se permiten reservas pasadas.");

            var horaLocal = fechaHoraUtc.ToLocalTime().TimeOfDay;
            if (!(horaLocal >= new TimeSpan(12, 0, 0) || horaLocal <= new TimeSpan(1, 0, 0)))
                throw new BusinessException("Horario VIP válido: 12:00 a 01:00.");

            if (reservaVipDto.CantidadPersonas < 1)
                throw new BusinessException("La cantidad mínima es 1");

            if (string.IsNullOrWhiteSpace(reservaVipDto.CodigoVip) || reservaVipDto.CodigoVip.Length < 6)
                throw new BusinessException("El Código VIP debe tener al menos 6 caracteres.");

            

            var cliente = await ObtenerOCrearClienteAsync(
                reservaVipDto.IdCliente, reservaVipDto.Nombre, reservaVipDto.Apellido, reservaVipDto.Email, reservaVipDto.Telefono, reservaVipDto.Cuit);

            var entity = _mapper.Map<Reserva>(reservaVipDto);

            entity.FechaHora = fechaHoraUtc;
            entity.ClienteId = cliente.Id;
            entity.UsuarioId = usuarioId;
            entity.TipoReservaId = (int)TipoReservaEnum.Vip;
            entity.EstadoReservaId = (int)EstadoReservaEnum.Confirmada;
            entity.FechaCreacion = DateTime.UtcNow;
            entity.FechaModificacion = DateTime.UtcNow;
            
            entity.DetalleReserva = new DetalleReserva
            {
                CodigoVip = reservaVipDto.CodigoVip,
                MesaId = reservaVipDto.MesaPreferidaId
            };

            var created = await _unitOfWork.ReservaRepository.CrearReservaVipAsync(entity);

            await _unitOfWork.SaveChanges();
            
            if (reservaVipDto.MesaPreferidaId.HasValue)
            {
                await _unitOfWork.ReservaRepository.AsignarMesasAsync(
                    created.Id,
                    new List<int> { reservaVipDto.MesaPreferidaId.Value });

                await _unitOfWork.SaveChanges();
            }

            return _mapper.Map<ReservaDTO>(created);
        }
        
        public async Task<ReservaDTO> CrearReservaCumpleAsync(ReservaCreateCumpleDTO reservaCumpleDto, int usuarioId)
        {
            var fechaHoraUtc = ParseFechaHora(reservaCumpleDto.FechaHora);

            if (fechaHoraUtc <= DateTime.UtcNow)
                throw new BusinessException("No se permiten reservas pasadas.");

            if (reservaCumpleDto.CantidadPersonas < 5 || reservaCumpleDto.CantidadPersonas > 12)
                throw new BusinessException("Cumple: entre 5 y 12 personas.");

            if (reservaCumpleDto.TraeTorta && fechaHoraUtc < DateTime.UtcNow.AddHours(48))
                throw new BusinessException("Con torta: debe reservarse 48 horas antes.");

            var horaLocal = fechaHoraUtc.ToLocalTime().TimeOfDay;
            if (horaLocal > new TimeSpan(23, 0, 0))
                throw new BusinessException("Máximo permitido: 23:00.");
            

            var cliente = await ObtenerOCrearClienteAsync(
                reservaCumpleDto.IdCliente, reservaCumpleDto.Nombre, reservaCumpleDto.Apellido, reservaCumpleDto.Email, reservaCumpleDto.Telefono, reservaCumpleDto.Cuit);

            var entity = _mapper.Map<Reserva>(reservaCumpleDto);

            entity.FechaHora = fechaHoraUtc;
            entity.ClienteId = cliente.Id;
            entity.UsuarioId = usuarioId;
            entity.TipoReservaId = (int)TipoReservaEnum.Cumpleanios;
            entity.EstadoReservaId = (int)EstadoReservaEnum.Pendiente;
            
            entity.DetalleReserva = new DetalleReserva
            {
                EdadCumpleaniero = reservaCumpleDto.EdadCumpleaniero,
                TraeTorta = reservaCumpleDto.TraeTorta
            };

            var created = await _unitOfWork.ReservaRepository.CrearReservaCumpleAsync(entity);

            await _unitOfWork.SaveChanges();

            return _mapper.Map<ReservaDTO>(created);
        }
        
        public async Task<ReservaDTO> ConfirmarReservaAsync(int id)
        {
            var updated = await _unitOfWork.ReservaRepository.ConfirmarReservaAsync(id);

            await _unitOfWork.SaveChanges();

            return _mapper.Map<ReservaDTO>(updated);
        }

        public async Task<ReservaDTO> CancelarReservaAsync(int id, string obs)
        {
            var updated = await _unitOfWork.ReservaRepository.CancelarReservaAsync(id, obs);

            await _unitOfWork.SaveChanges();
            return _mapper.Map<ReservaDTO>(updated);
        }

        public async Task<ReservaDTO> MarcarNoAsistioAsync(int id)
        {
            var updated = await _unitOfWork.ReservaRepository.MarcarNoAsistioAsync(id);

            await _unitOfWork.SaveChanges();
            return _mapper.Map<ReservaDTO>(updated);
        }

        public async Task<List<int>> AsignarMesasAsync(AsignarMesasRequestDTO mesasDto)
        {
            var mesas = await _unitOfWork.ReservaRepository.AsignarMesasAsync(mesasDto.ReservaId, mesasDto.MesaIds);
            await _unitOfWork.SaveChanges();
            return mesas.Select(x => x.MesaId).ToList();
        }

        public async Task<ReservaDetailDTO> GetByIdAsync(int id)
        {
            var res = await _unitOfWork.ReservaRepository.GetByIdAsync(id);
            return _mapper.Map<ReservaDetailDTO>(res);
        }

        public async Task<List<ReservaListItemDTO>> GetAllAsync(
            int page = 1, int pageSize = 20,
            DateTime? fecha = null,
            int? tipoReservaId = null,
            int? estadoReservaId = null,
            string? nombre = null,
            string? email = null)
        {
            var list = await _unitOfWork.ReservaRepository.GetAllAsync(page, pageSize, fecha, tipoReservaId, estadoReservaId, nombre, email);
            return _mapper.Map<List<ReservaListItemDTO>>(list);
        }
        
        private DateTime ParseFechaHora(string fechaHora)
        {
            if (!DateTime.TryParseExact(
                fechaHora, "dd/MM/yyyy HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeLocal,
                out DateTime dtLocal))
            {
                throw new BusinessException("Formato fecha inválido. Use 'dd/MM/yyyy HH:mm:ss'.");
            }

            return dtLocal.ToUniversalTime();
        }        

        private async Task<Cliente> ObtenerOCrearClienteAsync(
            int? idCliente,
            string nombre,
            string apellido,
            string email,
            string telefono, 
            string cuit)
        {
            Cliente cliente = null;

            if (idCliente.HasValue)
            {
                cliente = await _unitOfWork.ClienteRepository.GetById(idCliente.Value);                

                if (cliente != null)
                    return cliente;
                        
            }
            else
            {
                cliente = new Cliente
                {                    
                    Nombre = nombre,
                    Apellido = apellido,
                    Email = email,
                    Telefono = telefono,
                    Cuit = cuit,
                    EsPersonaFisica = true
                };

                await _unitOfWork.ClienteRepository.InsertCliente(cliente);
                await _unitOfWork.SaveChanges();
            }     

            return cliente;
        }
    }
}




