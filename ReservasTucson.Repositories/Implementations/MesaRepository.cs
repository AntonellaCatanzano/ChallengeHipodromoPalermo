using Microsoft.EntityFrameworkCore;
using ReservasTucson.DataAccess;
using ReservasTucson.Domain.Entities;
using ReservasTucson.Domain.Enums;
using ReservasTucson.Repositories.Interfaces;


namespace ReservasTucson.Repositories.Implementations
{
    public class MesaRepository : IMesaRepository
    {
        private readonly ReservasTucsonDBContext _dbContext;

        public MesaRepository(ReservasTucsonDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Mesa>> GetAll()
        {
            var mesas = await _dbContext.Mesas.ToListAsync();

            return mesas;
        }

        public async Task<Mesa> GetById(int id)
        {
            var mesa = await _dbContext.Mesas.FindAsync(id);

            return mesa;
        }

        public async Task<Mesa> InsertMesa(Mesa entity)
        {
            await _dbContext.Mesas.AddAsync(entity);

            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<List<Mesa>> GetDisponiblesAsync(
        DateTime fechaHora,
        int duracionMinutos,
        bool? soloVip = null)
        {
            var mesas = _dbContext.Mesas
                .Where(m => m.Activa);

            if (soloVip.HasValue)
                mesas = mesas.Where(m => m.EsVip == soloVip.Value);

            var mesasList = await mesas.ToListAsync();

            var reservas = await _dbContext.Reservas
                .Include(r => r.TipoReserva)
                .Include(r => r.ReservasMesas)
                .Where(r =>
                    r.EstadoReservaId == (int)EstadoReservaEnum.Confirmada ||
                    r.EstadoReservaId == (int)EstadoReservaEnum.Pendiente)
                .ToListAsync();

            return mesasList.Where(mesa =>
                !reservas.Any(r =>
                    r.ReservasMesas.Any(rm =>
                        rm.MesaId == mesa.Id &&
                        Overlaps(
                            fechaHora,
                            duracionMinutos,
                            r.FechaHora,
                            r.TipoReserva.TiempoPermanenciaMinutos
                        ))))
                .ToList();
        }

        public async Task<bool> EstaDisponibleAsync(
        int mesaId,
        DateTime fechaHora,
        int duracionMinutos)
        {
            var reservas = await _dbContext.Reservas
                .Include(r => r.TipoReserva)
                .Include(r => r.ReservasMesas)
                .Where(r =>
                    r.EstadoReservaId == (int)EstadoReservaEnum.Confirmada ||
                    r.EstadoReservaId == (int)EstadoReservaEnum.Pendiente)
                .ToListAsync();

            return !reservas.Any(r =>
                r.ReservasMesas.Any(rm =>
                    rm.MesaId == mesaId &&
                    Overlaps(
                        fechaHora,
                        duracionMinutos,
                        r.FechaHora,
                        r.TipoReserva.TiempoPermanenciaMinutos
                    )));
        }

        private bool Overlaps(DateTime startA, int durA, DateTime startB, int durB)
        {
            return startA < startB.AddMinutes(durB)
                && startB < startA.AddMinutes(durA);
        }
    }
}
