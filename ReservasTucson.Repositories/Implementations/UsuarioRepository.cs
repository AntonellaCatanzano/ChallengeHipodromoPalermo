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
            var usuario = await _dbContext.Usuarios.FindAsync(id);             

            return usuario;
        }

        public async Task<Usuario> InsertUsuario(Usuario entity)
        {
            // Verificar si ya existe un usuario con el mismo correo, nombre o apellido
            var existingUser = await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.Email == entity.Email);

            // Si el usuario ya existe, lanzamos una excepción.

            string value = Formatter.Base64Encode(entity.PasswordHash ?? "123456"); // Contraseña por defecto

            entity.PasswordHash = value;

            if (existingUser == null)
            {
                var usuario = await _dbContext.Usuarios.AddAsync(entity);

                await _dbContext.SaveChangesAsync();

                return entity;
            }
            else
            {
                throw new Exception("El email ingresado ya existe");
            }
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
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
