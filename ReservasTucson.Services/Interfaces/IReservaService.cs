using ReservasTucson.Domain.DTO;
using ReservasTucson.Domain.Entities;

public interface IReservaService
{
    // Creación de reservas
    Task<ReservaDTO> CrearReservaEstandarAsync(ReservaCreateStandardDTO reservaStandardDto, int usuarioId);
    Task<ReservaDTO> CrearReservaVipAsync(ReservaCreateVipDTO reservaVipDto, int usuarioId);
    Task<ReservaDTO> CrearReservaCumpleAsync(ReservaCreateCumpleDTO reservaCumpleDto, int usuarioId);

    // Cambios de estado
    Task<ReservaDTO> ConfirmarAsync(int reservaIdint, int usuarioId);
    Task<ReservaDTO> CancelarAsync(int reservaId, string observacion, int usuarioId);
    Task<ReservaDTO> MarcarNoAsistioAsync(int reservaId, int usuarioId);

    // Listados Paginados 
    Task<PaginatedList<ReservaListItemDTO>> GetReservasPaginadasAsync(ReservaFilterDTO filtros, int pageNumber, int pageSize);
    Task<ReservaDetailDTO> GetDetalleReservaAsync(int reservaId);
    Task<List<ReservaDTO>> GetAllAsync();

    Task<ReservaDTO> GetByIdAsync(int id);

}
