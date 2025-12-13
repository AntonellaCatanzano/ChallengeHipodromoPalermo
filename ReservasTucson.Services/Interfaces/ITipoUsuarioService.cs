
using ReservasTucson.Domain.DTO;


namespace ReservasTucson.Services.Interfaces
{
    public interface ITipoUsuarioService
    {
        Task<TipoUsuarioDTO> InsertTipoUsuario(TipoUsuarioDTO entity);
        Task<List<TipoUsuarioDTO>> GetAll();
        Task<TipoUsuarioDTO> GetById(int id);
    }
}
