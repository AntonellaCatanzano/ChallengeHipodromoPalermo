
using Microsoft.EntityFrameworkCore;
using ReservasTucson.DataAccess;
using ReservasTucson.Domain.Entities;
using ReservasTucson.Repositories.Interfaces;

namespace ReservasTucson.Repositories.Implementations
{
    public class DetalleReservaRepository : IDetalleReservaRepository
    {
        private readonly ReservasTucsonDBContext _dbContext;

        public DetalleReservaRepository(ReservasTucsonDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<DetalleReserva>> GetAll()
        {
            return await _dbContext.DetalleReservas
                 .Include(r => r.Reserva).ToListAsync();                 
                 
        }

        public async Task<DetalleReserva> GetById(int id)
        {
            return await _dbContext.DetalleReservas
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<DetalleReserva> GetByReservaId(int reservaId)
        {
            var detalleReserva = await _dbContext.DetalleReservas
                .Include(x => x.Reserva)
                .Where(x => x.Reserva.Id == reservaId)
                .FirstOrDefaultAsync();

            return detalleReserva;
        }
    }
}
