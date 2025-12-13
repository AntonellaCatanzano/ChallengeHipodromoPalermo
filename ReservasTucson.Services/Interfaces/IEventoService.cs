
using ReservasTucson.Domain.DTO;

namespace ReservasTucson.Services.Interfaces
{
    public interface IEventoService
    {
        Task<EventoDTO> InsertEvento(EventoDTO entity);
        Task<List<EventoDTO>> GetAll();
        Task<EventoDTO> GetById(int id);
    }
}
