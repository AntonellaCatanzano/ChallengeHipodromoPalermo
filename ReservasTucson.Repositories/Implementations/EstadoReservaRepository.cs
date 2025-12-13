using Microsoft.EntityFrameworkCore;
using ReservasTucson.DataAccess;
using ReservasTucson.Domain.Entities;
using ReservasTucson.Repositories.Interfaces;


namespace ReservasTucson.Repositories.Implementations
{
    public class EstadoReservaRepository : IEstadoReservaRepository
    {
        private readonly ReservasTucsonDBContext _dbContext;

        public EstadoReservaRepository(ReservasTucsonDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<EstadoReserva>> GetAll()
        {
            var estadosReservas = await _dbContext.EstadosReservas.ToListAsync();

            return estadosReservas;
        }

        public async Task<EstadoReserva> GetById(int id)
        {
            var estadoReserva = await _dbContext.EstadosReservas.FindAsync(id);

            return estadoReserva;
        }

        public async Task<EstadoReserva> InsertEstadoReserva(EstadoReserva entity)
        {
            await _dbContext.EstadosReservas.AddAsync(entity);

            await _dbContext.SaveChangesAsync();

            return entity;
        }
    }
}
