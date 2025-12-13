using Microsoft.EntityFrameworkCore;
using ReservasTucson.DataAccess;
using ReservasTucson.Domain.Entities;
using ReservasTucson.Domain.Support.Helpers;
using ReservasTucson.Repositories.Interfaces;
using System.Runtime.Serialization;

namespace ReservasTucson.Repositories.Implementations
{
    public class EventoRepository : IEventoRepository
    {
        private readonly ReservasTucsonDBContext _dbContext;

        public EventoRepository(ReservasTucsonDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Evento>> GetAll()
        {
            var eventos = await _dbContext.Eventos.ToListAsync();

            return eventos;
        }

        public async Task<Evento> GetById(int id)
        {
            var evento = await _dbContext.Eventos.FindAsync(id);

            return evento;
        }

        public async Task<Evento> InsertEvento(Evento entity)
        {
            await _dbContext.Eventos.AddAsync(entity);

            await _dbContext.SaveChangesAsync();

            return entity;
        }
    }
}
