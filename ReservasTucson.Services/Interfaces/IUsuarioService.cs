
using ReservasTucson.Domain.DTO;

namespace ReservasTucson.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDTO>> GetAll();
        Task<UsuarioDTO> GetById(int id);
        Task<UsuarioDTO> InsertUsuario(UsuarioDTO entity);
        Task<List<UsuarioDTO>> GetByTipoUsuario(int tipoUsuarioId);


    }
}
