using Microsoft.EntityFrameworkCore;
using ReservasTucson.DataAccess;
using ReservasTucson.Domain.Entities;
using ReservasTucson.Domain.Support.Helpers;
using ReservasTucson.Repositories.Interfaces;


namespace ReservasTucson.Repositories.Implementations
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ReservasTucsonDBContext _dbContext;

        public UsuarioRepository(ReservasTucsonDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Usuario>> GetAll()
        {
            var usuarios = await _dbContext.Usuarios
                .Include(tp => tp.TipoUsuario)
                .ToListAsync();

            return usuarios;
        }

        public async Task<Usuario> GetById(int id)
        {
            var usuario = await _dbContext.Usuarios
                .Include(u => u.TipoUsuario)
                .FirstOrDefaultAsync(u => u.IdUsuario == id);

            return usuario;
        }

        public async Task<Usuario> InsertUsuarioAsync(Usuario usuario)
        {
            _dbContext.Usuarios.Add(usuario);

            await _dbContext.SaveChangesAsync();

            return usuario;
        }

        public async Task<List<Usuario>> GetByTipoUsuario(int tipoUsuarioId)
        {
            var tiposUsuario = await _dbContext.Usuarios
                .Include(x => x.TipoUsuario)
                .Where(x => x.TipoUsuario.Id == tipoUsuarioId)
                .ToListAsync();

            return tiposUsuario;
        }

        public async Task<Usuario?> GetByEmail(string email)
        {
            return await _dbContext.Usuarios
                .Include(u => u.TipoUsuario)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> AnyAsync()
        {
            return await _dbContext.Usuarios.AnyAsync();
        }

        
    }
}
