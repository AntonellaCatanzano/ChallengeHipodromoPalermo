using Microsoft.EntityFrameworkCore;
using ReservasTucson.DataAccess;
using ReservasTucson.Domain.Entities;
using ReservasTucson.Repositories.Interfaces;

namespace ReservasTucson.Repositories.Implementations
{
    public class ReservaMesaRepository : IReservaMesaRepository
    {
        private readonly ReservasTucsonDBContext _dbContext;

        public ReservaMesaRepository(ReservasTucsonDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ReservaMesa>> GetAll()
        {
            var reservasMesas = await _dbContext.ReservasMesas
                 .Include(r => r.Reserva)
                 .Include(m => m.Mesa)
                 .ToListAsync();

            return reservasMesas;

        }

        public async Task<ReservaMesa> GetById(int id)
        {
            var reservaMesa = await _dbContext.ReservasMesas.FirstOrDefaultAsync(rm => rm.Id == id);

            return reservaMesa;
        }

        public async Task<List<ReservaMesa>> GetMesasByReservaId(int reservaId)
        {
            var mesas = await _dbContext.ReservasMesas
                .Include(m => m.Mesa)
                .Include(r => r.Reserva)
                .Where(r => r.Reserva.Id == reservaId)
                .ToListAsync();

            return mesas; 
        }

        public async Task<ReservaMesa> GetByReservaId(int reservaId)
        {
            var reserva = await _dbContext.ReservasMesas
                .Include(r => r.Reserva)
                .Where(r => r.Reserva.Id == reservaId)
                .FirstOrDefaultAsync();

            return reserva;
        }
    }
}
