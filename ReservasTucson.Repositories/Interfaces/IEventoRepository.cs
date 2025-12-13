using ReservasTucson.Domain.Entities;
using System.Threading.Tasks;

namespace ReservasTucson.Repositories.Interfaces
{
    public interface IEventoRepository
    {
        Task<Evento> InsertEvento(Evento entity);
        Task<List<Evento>> GetAll();
        Task<Evento> GetById(int id);
    }
}
