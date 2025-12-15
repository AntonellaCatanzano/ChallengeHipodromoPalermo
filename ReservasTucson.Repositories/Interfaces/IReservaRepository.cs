using ReservasTucson.Domain.Entities;

namespace ReservasTucson.Repositories.Interfaces
{
    public interface IReservaRepository
    {     

        Task<Reserva> AddAsync(Reserva entity);
        Task<Reserva?> GetByIdAsync(int id);
        Task<List<Reserva>> GetAllAsync();
        Task UpdateAsync(Reserva entity);       
    }
}
