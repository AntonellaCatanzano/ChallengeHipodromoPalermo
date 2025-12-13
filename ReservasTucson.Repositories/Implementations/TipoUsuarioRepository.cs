
using Microsoft.EntityFrameworkCore;
using ReservasTucson.DataAccess;
using ReservasTucson.Domain.Entities;
using ReservasTucson.Repositories.Interfaces;

namespace ReservasTucson.Repositories.Implementations
{
    public class TipoUsuarioRepository : ITipoUsuarioRepository
    {
        private readonly ReservasTucsonDBContext _dbContext;

        public TipoUsuarioRepository(ReservasTucsonDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TipoUsuario>> GetAll()
        {
            var tiposUsuarios = await _dbContext.TiposUsuario.ToListAsync();

            return tiposUsuarios;
        }

        public async Task<TipoUsuario> GetById(int id)
        {
            var tipoUsuario = await _dbContext.TiposUsuario.FindAsync(id);

            return tipoUsuario;
        }

        public async Task<TipoUsuario> InsertTipoUsuario(TipoUsuario entity)
        {
            await _dbContext.TiposUsuario.AddAsync(entity);

            await _dbContext.SaveChangesAsync();

            return entity;
        }
    }
}
