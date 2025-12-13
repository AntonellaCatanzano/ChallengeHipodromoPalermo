
using ReservasTucson.Domain.DTO;

namespace ReservasTucson.Services.Interfaces
{
    public interface ITipoReservaService
    {
        Task<TipoReservaDTO> InsertTipoReserva(TipoReservaDTO entity);
        Task<List<TipoReservaDTO>> GetAll();
        Task<TipoReservaDTO> GetById(int id);
    }
}
