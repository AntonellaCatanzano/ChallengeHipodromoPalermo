using ReservasTucson.Domain.Entities;
using System.Threading.Tasks;

namespace ReservasTucson.Repositories.Interfaces
{
    public interface ITipoUsuarioRepository
    {
        Task<TipoUsuario> InsertTipoUsuario(TipoUsuario entity);
        Task<List<TipoUsuario>> GetAll();
        Task<TipoUsuario> GetById(int id);
    }
}
