using ReservasTucson.Domain.Entities;
using System.Threading.Tasks;

namespace ReservasTucson.Repositories.Interfaces
{
    public interface IMesaRepository
    {
        Task<Mesa> InsertMesa(Mesa entity);
        Task<List<Mesa>> GetAll();
        Task<Mesa> GetById(int id);
    }
}
