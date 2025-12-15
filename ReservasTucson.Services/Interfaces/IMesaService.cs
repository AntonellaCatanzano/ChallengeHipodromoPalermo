
using ReservasTucson.Domain.DTO;

namespace ReservasTucson.Services.Interfaces
{
    public interface IMesaService
    {
        Task<MesaDTO> InsertMesa(MesaDTO entity);
        Task<List<MesaDTO>> GetAll();
        Task<MesaDTO> GetById(int id);

        Task<bool> EstaDisponibleAsync(
            int mesaId,
            DateTime fechaHora,
            int duracionMinutos,
            int? reservaIdExcluir = null);

        Task<List<MesaDTO>> GetMesasDisponiblesAsync(
            DateTime fechaHora,
            int duracionMinutos,
            int cantidadPersonas,
            bool soloVip);        
    }
    
}
