
using ReservasTucson.Domain.DTO;

namespace ReservasTucson.Services.Interfaces
{
    public interface IReservaMesaService
    {
        Task<List<ReservaMesaDTO>> GetAll();
        Task<ReservaMesaDTO> GetById(int id);
        Task<List<ReservaMesaDTO>> GetMesasByReservaId(int reservaId);
        Task<ReservaMesaDTO> GetByReservaId(int reservaId);
    }
}
