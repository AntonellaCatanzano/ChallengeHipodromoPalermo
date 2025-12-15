
using ReservasTucson.Domain.DTO;
using ReservasTucson.Domain.Entities;

namespace ReservasTucson.Services.Interfaces
{
    public interface IClienteService
    {
        Task<ClienteDTO> InsertCliente(ClienteDTO entity);
        Task<List<ClienteDTO>> GetAll();
        Task<ClienteDTO> GetById(int id);
        Task<ClienteDTO> GetByEmail(string email);
        Task<ClienteDTO> GetByEmailOrCuitAsync(string email, string cuit);
    }
}
