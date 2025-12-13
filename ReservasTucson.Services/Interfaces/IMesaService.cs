
using ReservasTucson.Domain.DTO;

namespace ReservasTucson.Services.Interfaces
{
    public interface IMesaService
    {
        Task<MesaDTO> InsertMesa(MesaDTO entity);
        Task<List<MesaDTO>> GetAll();
        Task<MesaDTO> GetById(int id);
    }
}
