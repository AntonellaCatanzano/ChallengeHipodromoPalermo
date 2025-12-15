
using ReservasTucson.Domain.Entities;

namespace ReservasTucson.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario> InsertUsuarioAsync(Usuario entity);
        
        Task<List<Usuario>> GetAll();
        Task<Usuario> GetById(int id);
        Task<List<Usuario>> GetByTipoUsuario(int tipoUsuarioId);
        Task<Usuario?> GetByEmail(string email);
        Task<bool> AnyAsync();
    }
}
