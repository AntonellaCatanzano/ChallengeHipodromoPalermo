using ReservasTucson.Domain.DTO;

public interface IReservaService
{
    // Creación de reservas
    Task<ReservaDTO> CrearReservaEstandarAsync(ReservaCreateStandardDTO reservaStandardDto, int usuarioId);
    Task<ReservaDTO> CrearReservaVipAsync(ReservaCreateVipDTO reservaVipDto, int usuarioId);
    Task<ReservaDTO> CrearReservaCumpleAsync(ReservaCreateCumpleDTO reservaCumpleDto, int usuarioId);

    // Cambios de estado
    Task<ReservaDTO> ConfirmarReservaAsync(int reservaId);
    Task<ReservaDTO> CancelarReservaAsync(int reservaId, string observacion);
    Task<ReservaDTO> MarcarNoAsistioAsync(int reservaId);
    
    Task<ReservaDetailDTO> GetByIdAsync(int id);

    Task<List<ReservaListItemDTO>> GetAllAsync(
        int page = 1,
        int pageSize = 20,
        DateTime? fecha = null,
        int? tipoReservaId = null,
        int? estadoReservaId = null,
        string? nombre = null,
        string? email = null
    );

    
    Task<List<int>> AsignarMesasAsync(AsignarMesasRequestDTO mesasDto);
}
