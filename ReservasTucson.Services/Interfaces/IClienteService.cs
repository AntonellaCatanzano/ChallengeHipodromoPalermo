
using ReservasTucson.Domain.DTO;

namespace ReservasTucson.Services.Interfaces
{
    public interface IClienteService
    {
        Task<ClienteDTO> InsertCliente(ClienteDTO entity);
        Task<List<ClienteDTO>> GetAll();
        Task<ClienteDTO> GetById(int id);
    }
}
