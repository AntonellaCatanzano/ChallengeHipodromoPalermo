

using Microsoft.EntityFrameworkCore;
using ReservasTucson.DataAccess;
using ReservasTucson.Domain.Entities;
using ReservasTucson.Repositories.Interfaces;

namespace ReservasTucson.Repositories.Implementations
{
    public class TipoReservaRepository : ITipoReservaRepository
    {
        private readonly ReservasTucsonDBContext _dbContext;

        public TipoReservaRepository(ReservasTucsonDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TipoReserva>> GetAll()
        {
            var mesas = await _dbContext.TiposReservas.ToListAsync();

            return mesas;
        }

        public async Task<TipoReserva> GetById(int id)
        {
            var mesa = await _dbContext.TiposReservas.FindAsync(id);

            return mesa;
        }

        public async Task<TipoReserva> InsertTipoReserva(TipoReserva entity)
        {
            await _dbContext.TiposReservas.AddAsync(entity);

            await _dbContext.SaveChangesAsync();

            return entity;
        }
    }
}
