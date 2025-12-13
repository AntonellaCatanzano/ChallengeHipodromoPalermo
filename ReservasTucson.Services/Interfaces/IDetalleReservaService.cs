
using ReservasTucson.Domain.DTO;

namespace ReservasTucson.Services.Interfaces
{
    public interface IDetalleReservaService
    {
        Task<List<DetalleReservaDTO>> GetAll();
        Task<DetalleReservaDTO> GetById(int id);
        Task<DetalleReservaDTO> GetByReservaId(int reservaId);
    }
}
