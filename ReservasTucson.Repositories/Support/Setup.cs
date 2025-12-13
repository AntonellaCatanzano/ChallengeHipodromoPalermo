
using Microsoft.Extensions.DependencyInjection;
using ReservasTucson.Repositories.Implementations;
using ReservasTucson.Repositories.Implementations.UoW;
using ReservasTucson.Repositories.Interfaces;
using ReservasTucson.Repositories.Interfaces.UoW;

namespace ReservasTucson.Repositories.Support
{
    public static class Setup
    {
        ///<summary>
        ///Método extensivo para la Configuración de la Base de Datos
        ///</summary>
        ///<param name="services"></param>
        ///<param name="configuration"></param>
        ///<returs></returs>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IClienteRepository, ClienteRepository>();
            services.AddTransient<IDetalleReservaRepository, DetalleReservaRepository>();
            services.AddTransient<IEstadoReservaRepository, EstadoReservaRepository>();
            services.AddTransient<IEventoRepository, EventoRepository>();
            services.AddTransient<IMesaRepository, MesaRepository>();
            services.AddTransient<IReservaMesaRepository, ReservaMesaRepository>();
            services.AddTransient<IReservaRepository, ReservaRepository>();
            services.AddTransient<ITipoReservaRepository, TipoReservaRepository>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<ITipoUsuarioRepository, TipoUsuarioRepository>();

            return services;
        }
    }
}
