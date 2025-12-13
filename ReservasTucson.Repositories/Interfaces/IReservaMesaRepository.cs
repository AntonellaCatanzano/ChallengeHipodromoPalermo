

using ReservasTucson.Domain.Entities;

namespace ReservasTucson.Repositories.Interfaces
{
    public interface IReservaMesaRepository
    {
        Task<List<ReservaMesa>> GetAll();
        Task<ReservaMesa> GetById(int id);
        Task<List<ReservaMesa>> GetMesasByReservaId(int reservaId);
        Task<ReservaMesa> GetByReservaId(int reservaId);
    }
}
