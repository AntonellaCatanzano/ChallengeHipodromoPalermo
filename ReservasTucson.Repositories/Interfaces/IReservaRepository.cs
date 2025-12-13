using ReservasTucson.Domain.Entities;

namespace ReservasTucson.Repositories.Interfaces
{
    public interface IReservaRepository
    {
        #region Creación de Reservas
        
        Task<Reserva> CrearReservaEstandarAsync(Reserva entity);        
        Task<Reserva> CrearReservaVipAsync(Reserva entity);
        
        Task<Reserva> CrearReservaCumpleAsync(Reserva entity);

        #endregion


        #region Cambios de Estado
        
        Task<Reserva> ConfirmarReservaAsync(int reservaId);
        
        Task<Reserva> CancelarReservaAsync(int reservaId, string observacion);
        
        Task<Reserva> MarcarNoAsistioAsync(int reservaId);

        #endregion        
        
        Task<Reserva> GetByIdAsync(int id);
        
        Task<List<Reserva>> GetAllAsync(
            int page = 1,
            int pageSize = 20,
            DateTime? fecha = null,
            int? tipoReservaId = null,
            int? estadoReservaId = null,
            string? nombre = null,
            string? email = null
        );       


        #region Mesas
        
        Task<List<ReservaMesa>> AsignarMesasAsync(int reservaId, List<int> mesaIds);

        #endregion
    }
}
