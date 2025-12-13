using Microsoft.EntityFrameworkCore.Storage;
using ReservasTucson.DataAccess;
using ReservasTucson.Repositories.Interfaces;
using ReservasTucson.Repositories.Interfaces.UoW;


namespace ReservasTucson.Repositories.Implementations.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ReservasTucsonDBContext _context;      
        

        public IClienteRepository ClienteRepository { get; }

        public IDetalleReservaRepository DetalleReservaRepository { get; }

        public IEstadoReservaRepository EstadoReservaRepository { get; }

        public IEventoRepository EventoRepository { get; }

        public IMesaRepository MesaRepository { get; }

        public IReservaMesaRepository ReservaMesaRepository { get; }

        public IReservaRepository ReservaRepository { get; }

        public ITipoReservaRepository TipoReservaRepository { get; }

        public IUsuarioRepository UsuarioRepository { get; }
        public ITipoUsuarioRepository TipoUsuarioRepository { get; }

        public UnitOfWork(ReservasTucsonDBContext context,
            IClienteRepository clienteRepository,
            IDetalleReservaRepository detalleReservaRepository,
            IEstadoReservaRepository estadoReservaRepository,
            IEventoRepository eventoRepository,
            IMesaRepository mesaRepository,
            IReservaMesaRepository reservaMesaRepository,
            IReservaRepository reservaRepository,
            ITipoReservaRepository tipoReservaRepository,
            IUsuarioRepository usuarioRepository,
            ITipoUsuarioRepository tipoUsuarioRepository)
            
        {
            this._context = context;

            this.ClienteRepository = clienteRepository;
            this.DetalleReservaRepository = detalleReservaRepository;
            this.EstadoReservaRepository = estadoReservaRepository;
            this.EventoRepository = eventoRepository;
            this.MesaRepository = mesaRepository;
            this.ReservaMesaRepository = reservaMesaRepository;
            this.ReservaRepository = reservaRepository;
            this.TipoReservaRepository = tipoReservaRepository;
            this.UsuarioRepository = usuarioRepository;
            this.TipoUsuarioRepository = tipoUsuarioRepository; 
            
        }


        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
        
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public void ChangeTrackerClear()
        {
            _context.ChangeTracker.Clear();
        }
    }
}
