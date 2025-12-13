
using ReservasTucson.Domain.DTO;

namespace ReservasTucson.Services.Interfaces
{
    public interface IEstadoReservaService
    {
        Task<EstadoReservaDTO> InsertEstadoReserva(EstadoReservaDTO entity);
        Task<List<EstadoReservaDTO>> GetAll();
        Task<EstadoReservaDTO> GetById(int id);
    }
}
