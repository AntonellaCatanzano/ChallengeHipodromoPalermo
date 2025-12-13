using ReservasTucson.Domain.Entities;
using System.Threading.Tasks;

namespace ReservasTucson.Repositories.Interfaces
{
    public interface ITipoReservaRepository
    {
        Task<TipoReserva> InsertTipoReserva(TipoReserva entity);
        Task<List<TipoReserva>> GetAll();
        Task<TipoReserva> GetById(int id);
    }
}
