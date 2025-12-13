using ReservasTucson.Domain.Entities;


namespace ReservasTucson.Repositories.Interfaces
{
    public interface IEstadoReservaRepository
    {
        Task<EstadoReserva> InsertEstadoReserva(EstadoReserva entity);
        Task<List<EstadoReserva>> GetAll();
        Task<EstadoReserva> GetById(int id);
    }
}
