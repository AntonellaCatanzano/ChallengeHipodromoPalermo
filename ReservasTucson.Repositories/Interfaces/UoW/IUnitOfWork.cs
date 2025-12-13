using Microsoft.EntityFrameworkCore.Storage;


namespace ReservasTucson.Repositories.Interfaces.UoW
{
    public interface IUnitOfWork
    {  
        IClienteRepository ClienteRepository { get; }
        IDetalleReservaRepository DetalleReservaRepository { get; }
        IEstadoReservaRepository EstadoReservaRepository { get; }
        IEventoRepository EventoRepository { get; }
        IMesaRepository MesaRepository { get; }
        IReservaMesaRepository ReservaMesaRepository { get; }
        IReservaRepository ReservaRepository { get; }
        ITipoReservaRepository TipoReservaRepository { get; }
        IUsuarioRepository UsuarioRepository { get; }
        ITipoUsuarioRepository TipoUsuarioRepository { get; }        


        Task SaveChanges();

        /// <summary>
        /// Completa los campos de Auditoría</summary>
        /// <param name="user">Usuario Logueado</param>        
        /// <returns></returns>        

        void ChangeTrackerClear();

        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
