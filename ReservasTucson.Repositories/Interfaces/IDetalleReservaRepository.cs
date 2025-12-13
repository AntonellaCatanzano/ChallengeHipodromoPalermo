
using ReservasTucson.Domain.Entities;

namespace ReservasTucson.Repositories.Interfaces
{
    public interface IDetalleReservaRepository
    {        
        Task<List<DetalleReserva>> GetAll();
        Task<DetalleReserva> GetById(int id);
        Task<DetalleReserva> GetByReservaId(int reservaId);
    }
}
