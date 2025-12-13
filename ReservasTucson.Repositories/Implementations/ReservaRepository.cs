
using Microsoft.EntityFrameworkCore;
using ReservasTucson.DataAccess;
using ReservasTucson.Domain.Entities;
using ReservasTucson.Domain.Enums;
using ReservasTucson.Repositories.Interfaces;


namespace ReservasTucson.Repositories.Implementations
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly ReservasTucsonDBContext _dbContext;

        public ReservaRepository(ReservasTucsonDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        private bool Overlaps(DateTime startA, int durationA, DateTime startB, int durationB)
        {
            var endA = startA.AddMinutes(durationA);
            var endB = startB.AddMinutes(durationB);

            return startA < endB && startB < endA;
        }
        
        public async Task<Reserva> CrearReservaEstandarAsync(Reserva entity)
        {
            var cliente = await _dbContext.Clientes.FindAsync(entity.ClienteId)
                ?? throw new Exception("El cliente indicado no existe.");

            if (entity.FechaHora <= DateTime.UtcNow)
                throw new Exception("No se pueden crear reservas para fechas pasadas.");

            var hora = entity.FechaHora.TimeOfDay;
            if (hora < new TimeSpan(19, 0, 0) || hora > new TimeSpan(23, 30, 0))
                throw new Exception("Horario permitido para estándar: 19:00 a 23:30.");

            if (entity.CantidadPersonas is < 1 or > 4)
                throw new Exception("La cantidad permitida es entre 1 y 4.");

            entity.EstadoReservaId = (int)EstadoReservaEnum.Pendiente;
            entity.TipoReservaId = (int)TipoReservaEnum.Estandar;

            entity.FechaCreacion = DateTime.UtcNow;
            entity.FechaModificacion = DateTime.UtcNow;

            await _dbContext.Reservas.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }
        
        public async Task<Reserva> CrearReservaVipAsync(Reserva entity)
        {
            var cliente = await _dbContext.Clientes.FindAsync(entity.ClienteId)
                ?? throw new Exception("El cliente indicado no existe.");

            if (entity.FechaHora <= DateTime.UtcNow)
                throw new Exception("No se pueden crear reservas para fechas pasadas.");

            var hora = entity.FechaHora.TimeOfDay;

            bool valido =
                hora >= new TimeSpan(12, 0, 0) ||
                hora <= new TimeSpan(1, 0, 0);

            if (!valido)
                throw new Exception("Horario permitido VIP: 12:00 a 01:00.");

            if (string.IsNullOrWhiteSpace(entity.CodigoVip) || entity.CodigoVip.Length < 6)
                throw new Exception("El Código VIP debe tener mínimo 6 caracteres.");

            entity.TipoReservaId = (int)TipoReservaEnum.Vip;
            entity.EstadoReservaId = (int)EstadoReservaEnum.Confirmada;

            entity.FechaCreacion = DateTime.UtcNow;
            entity.FechaModificacion = DateTime.UtcNow;

            await _dbContext.Reservas.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }
        
        public async Task<Reserva> CrearReservaCumpleAsync(Reserva entity)
        {
            var cliente = await _dbContext.Clientes.FindAsync(entity.ClienteId)
                ?? throw new Exception("El Cliente no ha sido encontrado.");

            if (entity.FechaHora <= DateTime.UtcNow)
                throw new Exception("No se permiten reservas con fechas anteriores a la actual.");

            if (entity.CantidadPersonas < 5 || entity.CantidadPersonas > 12)
                throw new Exception("Cumpleaños: La cantidad maáxima de personas es entre 5 y 12.");

            if (entity.DetalleReserva?.TraeTorta == true)
                if (entity.FechaHora < DateTime.UtcNow.AddHours(48))
                    throw new Exception("Si requiere torta, debe realizarse con 48 horas de anticipación.");

            if (entity.FechaHora.TimeOfDay > new TimeSpan(23, 0, 0))
                throw new Exception("El Horario permitido cumpleaños es hasta las 23:00.");

            entity.TipoReservaId = (int)TipoReservaEnum.Cumpleanios;
            entity.EstadoReservaId = (int)EstadoReservaEnum.Pendiente;

            entity.FechaCreacion = DateTime.UtcNow;
            entity.FechaModificacion = DateTime.UtcNow;

            await _dbContext.Reservas.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }
        
        public async Task<Reserva> ConfirmarReservaAsync(int reservaId)
        {
            var reserva = await _dbContext.Reservas
                .Include(r => r.TipoReserva)
                .FirstOrDefaultAsync(r => r.Id == reservaId)
                ?? throw new Exception("La Reserva no ha sido encontrada.");

            if (reserva.EstadoReservaId != (int)EstadoReservaEnum.Pendiente)
                throw new Exception("Solo se confirma una reserva que se encuentra en estado pendiente.");

            if (reserva.FechaHora <= DateTime.UtcNow)
                throw new Exception("No puede confirmarse una reserva con fecha pasada.");

            int duracion = reserva.TipoReserva?.TiempoPermanenciaMinutos ?? 120;

            var mesasActuales = await _dbContext.ReservasMesas
                .Where(x => x.ReservaId == reservaId)
                .ToListAsync();

            foreach (var rm in mesasActuales)
            {
                var conflictos = await _dbContext.ReservasMesas
                    .Include(x => x.Reserva).ThenInclude(r => r.TipoReserva)
                    .Where(x => x.MesaId == rm.MesaId && x.ReservaId != reservaId)
                    .ToListAsync();

                foreach (var other in conflictos)
                {
                    int durOther = other.Reserva.TipoReserva?.TiempoPermanenciaMinutos ?? 120;

                    if (Overlaps(reserva.FechaHora, duracion, other.Reserva.FechaHora, durOther))
                        throw new Exception($"La mesa {rm.MesaId} está ocupada en ese horario.");
                }
            }

            reserva.EstadoReservaId = (int)EstadoReservaEnum.Confirmada;
            reserva.FechaModificacion = DateTime.UtcNow;

            _dbContext.Update(reserva);
            await _dbContext.SaveChangesAsync();

            return reserva;
        }       
        
        public async Task<List<ReservaMesa>> AsignarMesasAsync(int reservaId, List<int> mesaIds)
        {
            var reserva = await _dbContext.Reservas
                .Include(r => r.TipoReserva)
                .FirstOrDefaultAsync(r => r.Id == reservaId)
                ?? throw new Exception("La Reserva no ha sido encontrada.");

            if (reserva.FechaHora <= DateTime.UtcNow)
                throw new Exception("No se asignan mesas a reservas con fecha anterior a la actual.");

            int duracion = reserva.TipoReserva?.TiempoPermanenciaMinutos ?? 120;

            var mesas = await _dbContext.Mesas
                .Where(m => mesaIds.Contains(m.Id))
                .ToListAsync();

            if (mesas.Count != mesaIds.Count)
                throw new Exception("Alguna mesa no existe.");

            foreach (var mesa in mesas)
            {
                if (!mesa.Activa)
                    throw new Exception($"La mesa {mesa.Id} está inactiva.");
            }

            foreach (var mesaId in mesaIds)
            {
                var conflictos = await _dbContext.ReservasMesas
                    .Include(x => x.Reserva).ThenInclude(r => r.TipoReserva)
                    .Where(x => x.MesaId == mesaId && x.ReservaId != reservaId)
                    .ToListAsync();

                foreach (var other in conflictos)
                {
                    int durOther = other.Reserva.TipoReserva?.TiempoPermanenciaMinutos ?? 120;

                    if (Overlaps(reserva.FechaHora, duracion, other.Reserva.FechaHora, durOther))
                        throw new Exception($"La mesa {mesaId} no está disponible.");
                }
            }

            var actuales = await _dbContext.ReservasMesas
                .Where(rm => rm.ReservaId == reservaId)
                .ToListAsync();

            if (actuales.Any())
                _dbContext.ReservasMesas.RemoveRange(actuales);

            var nuevas = mesaIds
                .Select(id => new ReservaMesa
                {
                    ReservaId = reservaId,
                    MesaId = id,
                    Notas = "La Mesa ha sido asignada"
                }).ToList();

            await _dbContext.ReservasMesas.AddRangeAsync(nuevas);
            await _dbContext.SaveChangesAsync();

            return nuevas;
        }
        
        
        public async Task<Reserva> CancelarReservaAsync(int reservaId, string observacion)
        {
            var reserva = await _dbContext.Reservas.FindAsync(reservaId)
                ?? throw new Exception("La Reserva no ha sido encontrada.");

            if (reserva.EstadoReservaId is not (int)EstadoReservaEnum.Pendiente
                and not (int)EstadoReservaEnum.Confirmada)
                throw new Exception("Solo se cancela una reserva que se encuentra en estado pendiente o confirmada.");

            reserva.ObservacionCancelacion = observacion;
            reserva.EstadoReservaId = (int)EstadoReservaEnum.Cancelada;
            reserva.FechaModificacion = DateTime.UtcNow;

            _dbContext.Update(reserva);
            await _dbContext.SaveChangesAsync();

            return reserva;
        }
        
        public async Task<Reserva> MarcarNoAsistioAsync(int reservaId)
        {
            var reserva = await _dbContext.Reservas.FindAsync(reservaId)
                ?? throw new Exception("Reserva no encontrada.");

            if (reserva.EstadoReservaId != (int)EstadoReservaEnum.Confirmada)
                throw new Exception("Solo se marca 'No asistió' desde estado Confirmada.");

            reserva.EstadoReservaId = (int)EstadoReservaEnum.NoAsistio;
            reserva.FechaModificacion = DateTime.UtcNow;

            _dbContext.Update(reserva);
            await _dbContext.SaveChangesAsync();

            return reserva;
        }
        
        public async Task<Reserva> GetByIdAsync(int id)
        {
            return await _dbContext.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.TipoReserva)
                .Include(r => r.EstadoReserva)
                .Include(r => r.ReservasMesas).ThenInclude(rm => rm.Mesa)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
        
        public async Task<List<Reserva>> GetAllAsync(
            int page = 1,
            int pageSize = 20,
            DateTime? fecha = null,
            int? tipoReservaId = null,
            int? estadoReservaId = null,
            string? nombre = null,
            string? email = null)
        {
            var query = _dbContext.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.TipoReserva)
                .Include(r => r.EstadoReserva)
                .Include(r => r.ReservasMesas).ThenInclude(rm => rm.Mesa)
                .AsQueryable();

            if (fecha.HasValue)
                query = query.Where(r => r.FechaHora.Date == fecha.Value.Date);

            if (tipoReservaId.HasValue)
                query = query.Where(r => r.TipoReservaId == tipoReservaId.Value);

            if (estadoReservaId.HasValue)
                query = query.Where(r => r.EstadoReservaId == estadoReservaId.Value);

            if (!string.IsNullOrWhiteSpace(nombre))
                query = query.Where(r =>
                    (r.Cliente.Nombre + " " + r.Cliente.Apellido).Contains(nombre));

            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(r => r.Cliente.Email.Contains(email));

            return await query
                .OrderByDescending(r => r.FechaHora)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}

