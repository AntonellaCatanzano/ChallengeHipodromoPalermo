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

        public async Task<ReservaDTO> CrearReservaEstandarAsync(ReservaCreateStandardDTO reservaStandardDto, int usuarioId)
        {
            var usuario = await _unitOfWork.UsuarioRepository.GetById(usuarioId);

            if (usuario == null)
                throw new BusinessException("El usuario autenticado no existe.");

            if (!DateTime.TryParseExact(
                reservaStandardDto.FechaHora,                
                "yyyy-MM-dd HH:mm",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var fechaHora))
            {
                throw new BusinessException(
                    "Formato de fecha inválido. Use yyyy-MM-dd HH:mm");
            }

            if (fechaHora <= DateTime.Now)
                throw new BusinessException(
                    "No se pueden crear reservas para fechas pasadas.");

            var hora = fechaHora.TimeOfDay;
            if (hora < new TimeSpan(19, 0, 0) || hora > new TimeSpan(23, 30, 0))
                throw new BusinessException(
                    "Horario permitido: 19:00 a 23:30.");


            if (reservaStandardDto.CantidadPersonas < 1 || reservaStandardDto.CantidadPersonas > 4)
                throw new BusinessException(
                    "La cantidad de personas debe ser entre 1 y 4.");

            Cliente cliente;

            if (reservaStandardDto.IdCliente.HasValue)
            {
                cliente = await _unitOfWork.ClienteRepository
                    .GetById(reservaStandardDto.IdCliente.Value);

                if (cliente == null)
                    throw new BusinessException(
                        "El cliente indicado no existe.");
            }
            else
            {
                cliente = await _unitOfWork.ClienteRepository
                    .GetByEmailAsync(reservaStandardDto.Email.Trim());

                if (cliente == null)
                {
                    cliente = new Cliente
                    {
                        Nombre = reservaStandardDto.Nombre.Trim(),
                        Apellido = reservaStandardDto.Apellido.Trim(),
                        Email = reservaStandardDto.Email.Trim(),
                        Telefono = reservaStandardDto.Telefono?.Trim(),
                        Cuit = reservaStandardDto.Cuit.Trim()
                    };

                    cliente = await _unitOfWork.ClienteRepository
                        .InsertCliente(cliente);
                }
            }

            var reserva = new Reserva
            {
                ClienteId = cliente.Id,
                FechaHora = fechaHora,
                CantidadPersonas = reservaStandardDto.CantidadPersonas,
                Observaciones = reservaStandardDto.Observaciones,
                UsuarioId = usuarioId,
                TipoReservaId = (int)TipoReservaEnum.Estandar,
                EstadoReservaId = (int)EstadoReservaEnum.Pendiente,
                SeniaPagada = false,
                FechaCreacion = DateTime.UtcNow,
                FechaModificacion = DateTime.UtcNow
            };

            await _unitOfWork.ReservaRepository.AddAsync(reserva);

            return _mapper.Map<ReservaDTO>(reserva);
        }

        public async Task<ReservaDTO> CrearReservaVipAsync(ReservaCreateVipDTO reservaVipDto, int usuarioId)
        {
            var usuario = await _unitOfWork.UsuarioRepository.GetById(usuarioId);
            if (usuario == null)
                throw new BusinessException("El usuario autenticado no existe.");

            if (!DateTime.TryParseExact(
                reservaVipDto.FechaHora,
                "yyyy-MM-dd HH:mm",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var fechaHora))
                throw new BusinessException("Formato de fecha inválido. Use yyyy-MM-dd HH:mm");

            if (fechaHora <= DateTime.Now)
                throw new BusinessException("No se pueden crear reservas para fechas pasadas.");

            var hora = fechaHora.TimeOfDay;
            var horarioValido = hora >= new TimeSpan(12, 0, 0) || hora <= new TimeSpan(1, 0, 0);
            if (!horarioValido)
                throw new BusinessException("Horario permitido VIP: 12:00 a 01:00.");

            if (string.IsNullOrWhiteSpace(reservaVipDto.CodigoVip) || reservaVipDto.CodigoVip.Length < 6)
                throw new BusinessException("El Código VIP debe tener mínimo 6 caracteres.");
            
            Cliente cliente;
            if (reservaVipDto.IdCliente.HasValue)
            {
                cliente = await _unitOfWork.ClienteRepository.GetById(reservaVipDto.IdCliente.Value)
                         ?? throw new BusinessException("El cliente indicado no existe.");
            }
            else
            {
                cliente = await _unitOfWork.ClienteRepository.GetByEmailAsync(reservaVipDto.Email.Trim());
                if (cliente == null)
                {
                    cliente = new Cliente
                    {
                        Nombre = reservaVipDto.Nombre.Trim(),
                        Apellido = reservaVipDto.Apellido.Trim(),
                        Email = reservaVipDto.Email.Trim(),
                        Telefono = reservaVipDto.Telefono?.Trim(),
                        Cuit = reservaVipDto.Cuit.Trim()
                    };
                    cliente = await _unitOfWork.ClienteRepository.InsertCliente(cliente);
                }
            }

            MesaDTO? mesaAsignada = null;
            
            if (reservaVipDto.MesaPreferidaId.HasValue)
            {
                var mesaPref = await _unitOfWork.MesaRepository.GetById(reservaVipDto.MesaPreferidaId.Value);
                if (mesaPref == null)
                    throw new BusinessException("La mesa preferida indicada no existe.");

                var disponible = await _unitOfWork.MesaRepository.EstaDisponibleAsync(
                    mesaPref.Id, fechaHora, reservaVipDto.CantidadPersonas * 30);

                if (!disponible)
                    throw new BusinessException("La mesa preferida no está disponible para la fecha y hora indicada.");

                mesaAsignada = _mapper.Map<MesaDTO>(mesaPref);
            }
            
            if (mesaAsignada == null)
            {
                var mesasDisponibles = await _unitOfWork.MesaRepository.GetDisponiblesAsync(
                    fechaHora,
                    reservaVipDto.CantidadPersonas * 30,
                    soloVip: true
                );

                var mesaEntity = mesasDisponibles
                    .Where(m => m.Capacidad >= reservaVipDto.CantidadPersonas)
                    .OrderBy(m => m.Capacidad)
                    .FirstOrDefault();

                if (mesaEntity == null)
                    throw new BusinessException("No hay mesas VIP disponibles para la fecha y hora indicada.");

                mesaAsignada = _mapper.Map<MesaDTO>(mesaEntity);
            }

            // Crear reserva
            var reserva = new Reserva
            {
                ClienteId = cliente.Id,
                FechaHora = fechaHora,
                CantidadPersonas = reservaVipDto.CantidadPersonas,
                CodigoVip = reservaVipDto.CodigoVip,
                UsuarioId = usuarioId,
                TipoReservaId = (int)TipoReservaEnum.Vip,
                EstadoReservaId = (int)EstadoReservaEnum.Confirmada,
                Observaciones = reservaVipDto.Observaciones,
                FechaCreacion = DateTime.UtcNow,
                FechaModificacion = DateTime.UtcNow,
                DetalleReserva = new DetalleReserva
                {
                    CodigoVip = reservaVipDto.CodigoVip
                }
            };
            
            await _unitOfWork.ReservaRepository.AddAsync(reserva);
            await _unitOfWork.SaveChanges();

            
            if (mesaAsignada != null)
            {
                var reservaMesa = new ReservaMesa
                {
                    ReservaId = reserva.Id,
                    MesaId = mesaAsignada.Id,
                    Notas = "Asignada automáticamente o preferida"
                };

                await _unitOfWork.ReservaMesaRepository.AddAsync(reservaMesa);

                await _unitOfWork.SaveChanges();
            }

            return _mapper.Map<ReservaDTO>(reserva);
        }

        public async Task<ReservaDTO> CrearReservaCumpleAsync(ReservaCreateCumpleDTO reservaCumpleDto, int usuarioId)
        {            
            var usuario = await _unitOfWork.UsuarioRepository.GetById(usuarioId);

            if (usuario == null)
                throw new BusinessException("El usuario autenticado no existe.");

            // 2. Parse fecha
            if (!DateTime.TryParseExact(
                reservaCumpleDto.FechaHora,
                "yyyy-MM-dd HH:mm",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var fechaHora))
                throw new BusinessException("Formato de fecha inválido. Use yyyy-MM-dd HH:mm");

            if (fechaHora <= DateTime.Now)
                throw new BusinessException("No se pueden crear reservas para fechas pasadas.");

            // 3. Horario permitido
            if (fechaHora.TimeOfDay > new TimeSpan(23, 0, 0))
                throw new BusinessException("El horario permitido es hasta las 23:00.");

            
            if (reservaCumpleDto.CantidadPersonas < 5 || reservaCumpleDto.CantidadPersonas > 12)
                throw new BusinessException("Cantidad permitida: mínimo 5, máximo 12.");

            
            if (reservaCumpleDto.TraeTorta && fechaHora < DateTime.Now.AddHours(48))
                throw new BusinessException(
                    "Si requiere torta, la reserva debe realizarse con 48 horas de anticipación.");

            
            Cliente cliente;

            if (reservaCumpleDto.IdCliente.HasValue)
            {
                cliente = await _unitOfWork.ClienteRepository.GetById(reservaCumpleDto.IdCliente.Value)
                    ?? throw new BusinessException("El cliente indicado no existe.");
            }
            else
            {
                cliente = await _unitOfWork.ClienteRepository.GetByEmailAsync(reservaCumpleDto.Email.Trim());

                if (cliente == null)
                {
                    cliente = new Cliente
                    {
                        Nombre = reservaCumpleDto.Nombre.Trim(),
                        Apellido = reservaCumpleDto.Apellido.Trim(),
                        Email = reservaCumpleDto.Email.Trim(),
                        Telefono = reservaCumpleDto.Telefono?.Trim(),
                        Cuit = reservaCumpleDto.Cuit.Trim()
                    };

                    cliente = await _unitOfWork.ClienteRepository.InsertCliente(cliente);
                }
            }
            
            var reserva = new Reserva
            {
                ClienteId = cliente.Id,
                FechaHora = fechaHora,
                CantidadPersonas = reservaCumpleDto.CantidadPersonas,
                UsuarioId = usuarioId,
                TipoReservaId = (int)TipoReservaEnum.Cumpleanios,
                EstadoReservaId = (int)EstadoReservaEnum.Pendiente,
                FechaCreacion = DateTime.UtcNow,
                FechaModificacion = DateTime.UtcNow,
                Observaciones = reservaCumpleDto.Observaciones,
                DetalleReserva = new DetalleReserva
                {
                    EdadCumpleaniero = reservaCumpleDto.EdadCumpleaniero,
                    TraeTorta = reservaCumpleDto.TraeTorta
                }
            };

            await _unitOfWork.ReservaRepository.AddAsync(reserva);

            return _mapper.Map<ReservaDTO>(reserva);
        }

        public async Task<ReservaDTO> ConfirmarAsync(int reservaId, int usuarioId)
        {
            var usuario = await _unitOfWork.UsuarioRepository.GetById(usuarioId);

            if (usuario == null)
                throw new BusinessException("El usuario autenticado no existe.");
            
            var reserva = await _unitOfWork.ReservaRepository.GetByIdAsync(reservaId)
                ?? throw new BusinessException("La reserva no existe.");
            
            if (reserva.EstadoReservaId != (int)EstadoReservaEnum.Pendiente)
                throw new BusinessException("Solo se pueden confirmar reservas en estado Pendiente.");
            
            if (reserva.FechaHora <= DateTime.UtcNow)
                throw new BusinessException("No se puede confirmar una reserva con fecha pasada.");
            
            if (!reserva.ReservasMesas.Any())
            {
                var mesasDisponibles = await _unitOfWork.MesaRepository.GetDisponiblesAsync(
                    reserva.FechaHora,
                    reserva.TipoReserva.TiempoPermanenciaMinutos,
                    reserva.TipoReservaId == (int)TipoReservaEnum.Vip
                );

                var mesa = mesasDisponibles
                    .Where(m => m.Capacidad >= reserva.CantidadPersonas)
                    .OrderBy(m => m.Capacidad)
                    .FirstOrDefault();

                if (mesa == null)
                    throw new BusinessException("No hay mesas disponibles para la fecha y hora seleccionada.");

                reserva.ReservasMesas.Add(new ReservaMesa
                {
                    MesaId = mesa.Id,
                    ReservaId = reserva.Id
                });
            }
            
            foreach (var rm in reserva.ReservasMesas)
            {
                var disponible = await _unitOfWork.MesaRepository.EstaDisponibleAsync(
                    rm.MesaId,
                    reserva.FechaHora,
                    reserva.TipoReserva.TiempoPermanenciaMinutos
                );

                if (!disponible)
                    throw new BusinessException($"La mesa {rm.MesaId} no está disponible para la fecha y hora seleccionada.");
            }

            reserva.UsuarioId = usuarioId;
            reserva.EstadoReservaId = (int)EstadoReservaEnum.Confirmada;
            reserva.FechaModificacion = DateTime.UtcNow;

            await _unitOfWork.ReservaRepository.UpdateAsync(reserva);
            await _unitOfWork.SaveChanges();

            return _mapper.Map<ReservaDTO>(reserva);
        }

        public async Task<ReservaDTO> CancelarAsync(int reservaId, string observacion, int usuarioId)
        {
            var usuario = await _unitOfWork.UsuarioRepository.GetById(usuarioId);

            if (usuario == null)
                throw new BusinessException("El usuario autenticado no existe.");

            var reserva = await _unitOfWork.ReservaRepository.GetByIdAsync(reservaId)
                ?? throw new BusinessException("Reserva no encontrada.");

            // Criterio 1: Solo Pendiente o Confirmada
            if (reserva.EstadoReservaId is not ((int)EstadoReservaEnum.Pendiente or (int)EstadoReservaEnum.Confirmada))
                throw new BusinessException("No se puede cancelar en este estado.");

            // Criterio 2: Cambiar estado
            reserva.EstadoReservaId = (int)EstadoReservaEnum.Cancelada;

            // Criterio 3: Guardar observación
            reserva.UsuarioId = usuarioId;
            reserva.ObservacionCancelacion = observacion;
            reserva.FechaModificacion = DateTime.UtcNow;

            await _unitOfWork.ReservaRepository.UpdateAsync(reserva);
            await _unitOfWork.SaveChanges();

            return _mapper.Map<ReservaDTO>(reserva);
        }

        public async Task<ReservaDTO> MarcarNoAsistioAsync(int reservaId, int usuarioId)
        {
            var usuario = await _unitOfWork.UsuarioRepository.GetById(usuarioId);

            if (usuario == null)
                throw new BusinessException("El usuario autenticado no existe.");
            var reserva = await _unitOfWork.ReservaRepository.GetByIdAsync(reservaId)
                ?? throw new BusinessException("La reserva no existe.");

            // Criterio 1: Solo Confirmada
            if (reserva.EstadoReservaId != (int)EstadoReservaEnum.Confirmada)
                throw new BusinessException("Solo se puede marcar como No Asistió una reserva Confirmada.");

            // Criterio 2: Cambiar estado
            reserva.UsuarioId = usuarioId;
            reserva.EstadoReservaId = (int)EstadoReservaEnum.NoAsistio;
            reserva.FechaModificacion = DateTime.UtcNow;

            // Criterio 3: No permitir más cambios
            await _unitOfWork.ReservaRepository.UpdateAsync(reserva);
            await _unitOfWork.SaveChanges();

            return _mapper.Map<ReservaDTO>(reserva);
        }

        public async Task<PaginatedList<ReservaListItemDTO>> GetReservasPaginadasAsync(ReservaFilterDTO filtros, int pageNumber, int pageSize)
        {

            var reservas = await _unitOfWork.ReservaRepository.GetAllAsync();

            // filtros por nombre
            if (!string.IsNullOrWhiteSpace(filtros.Nombre))
                reservas = reservas
                    .Where(r => r.Cliente != null && r.Cliente.Nombre.Contains(filtros.Nombre, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            // filtros por email
            if (!string.IsNullOrWhiteSpace(filtros.Email))
                reservas = reservas
                    .Where(r => r.Cliente != null && r.Cliente.Email.Contains(filtros.Email, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            // filtros por tipo de reserva
            if (filtros.TipoReservaId.HasValue)
                reservas = reservas
                    .Where(r => r.TipoReservaId == filtros.TipoReservaId.Value)
                    .ToList();

            // filtros por estado
            if (filtros.EstadoReservaId.HasValue)
                reservas = reservas
                    .Where(r => r.EstadoReservaId == filtros.EstadoReservaId.Value)
                    .ToList();

            // filtro por fecha (string -> DateTime)
            if (!string.IsNullOrWhiteSpace(filtros.Fecha))
            {
                if (DateTime.TryParseExact(
                    filtros.Fecha,
                    "yyyy-MM-dd HH:mm",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var fechaHora))
                {
                    reservas = reservas
                        .Where(r => r.FechaHora.Date == fechaHora.Date)
                        .ToList();
                }
                else
                {
                    throw new BusinessException("Formato de fecha inválido. Use yyyy-MM-dd HH:mm");
                }
            }

            var total = reservas.Count;

            // Paginación
            var paged = reservas
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new PaginatedList<ReservaListItemDTO>(
                _mapper.Map<List<ReservaListItemDTO>>(paged),
                total,
                pageNumber,
                pageSize
            );

            return result;
        }
        

        public async Task<ReservaDetailDTO> GetDetalleReservaAsync(int reservaId)
        {
            var reserva = await _unitOfWork.ReservaRepository.GetByIdAsync(reservaId);
            if (reserva == null)
                throw new BusinessException("La reserva no existe.");

            var reservaDetalleDto = _mapper.Map<ReservaDetailDTO>(reserva);

            if (reserva.DetalleReserva != null)
            {
                reservaDetalleDto.DetalleReserva = _mapper.Map<DetalleReservaDTO>(reserva.DetalleReserva);
            }

            if (reserva.ReservasMesas != null && reserva.ReservasMesas.Any())
            {
                reservaDetalleDto.ReservasMesas = reserva.ReservasMesas
                    .Select(rm => _mapper.Map<ReservaMesaDTO>(rm))
                    .ToList();
            }

            return reservaDetalleDto;
        }

        public async Task<List<ReservaDTO>> GetAllAsync()
        {
            var reservas = await _unitOfWork.ReservaRepository.GetAllAsync();

            return _mapper.Map<List<ReservaDTO>>(reservas);
        }
        
        public async Task<ReservaDTO> GetByIdAsync(int id)
        {
            var reserva = await _unitOfWork.ReservaRepository.GetByIdAsync(id);

            if (reserva == null)
                throw new BusinessException("La reserva no existe.");

            return _mapper.Map<ReservaDTO>(reserva);
        }
    }

}


        

       





