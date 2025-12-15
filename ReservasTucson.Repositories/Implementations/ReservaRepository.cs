
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
        

        public async Task<Reserva> AddAsync(Reserva entity)
        {
            await _dbContext.Reservas.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Reserva entity)
        {
            _dbContext.Reservas.Update(entity);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<Reserva?> GetByIdAsync(int id)
        {
            return await _dbContext.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.DetalleReserva)
                .Include(r => r.EstadoReserva)
                .Include(r => r.TipoReserva)
                .Include(r => r.ReservasMesas)
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Reserva>> GetAllAsync()
        {
            return await _dbContext.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.DetalleReserva)
                .Include(r => r.EstadoReserva)
                .Include(r => r.TipoReserva)
                .Include(r => r.ReservasMesas)
                .Include(r => r.Usuario)
                .ToListAsync();
        }        
    }
}

