using Microsoft.EntityFrameworkCore;
using ReservasTucson.DataAccess;
using ReservasTucson.Domain.Entities;
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
    }
}
