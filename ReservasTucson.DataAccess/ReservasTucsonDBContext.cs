using Microsoft.EntityFrameworkCore;
using ReservasTucson.Domain.Builders;
using ReservasTucson.Domain.Entities;

namespace ReservasTucson.DataAccess
{
    public class ReservasTucsonDBContext : DbContext
    {
        public ReservasTucsonDBContext(DbContextOptions<ReservasTucsonDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(ClienteTypeConfiguration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(DetalleReservaTypeConfiguration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(EstadoReservaTypeConfiguration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(EventoTypeConfiguration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(MesaTypeConfiguration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(ReservaMesaTypeConfiguration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(ReservaTypeConfiguration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(TipoReservaTypeConfiguration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(UsuarioTypeConfiguration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(TipoUsuarioTypeConfiguration).Assembly); 

            base.OnModelCreating(builder);
        }

        // DbSet Properties
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<DetalleReserva> DetalleReservas { get; set; }
        public DbSet<EstadoReserva> EstadosReservas { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<ReservaMesa> ReservasMesas { get; set; }
        public DbSet<TipoReserva> TiposReservas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<TipoUsuario> TiposUsuario { get; set; } 
    }
}
