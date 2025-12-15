using ReservasTucson.Domain.Entities;
using System.Threading.Tasks;

namespace ReservasTucson.Repositories.Interfaces
{
    public interface IMesaRepository
    {
        Task<Mesa> InsertMesa(Mesa entity);
        Task<List<Mesa>> GetAll();
        Task<Mesa> GetById(int id);

        Task<List<Mesa>> GetDisponiblesAsync(DateTime fechaHora, int duracionMinutos, bool? soloVip = null);


        Task<bool> EstaDisponibleAsync(int mesaId, DateTime fechaHora, int duracionMinutos);
    }
}
