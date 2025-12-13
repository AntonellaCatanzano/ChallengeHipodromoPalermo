using Microsoft.Extensions.DependencyInjection;
using ReservasTucson.Services.Implementations.Auth;
using ReservasTucson.Services.Interfaces.Auth;
using ReservasTucson.Services.Interfaces;
using ReservasTucson.Services.Implementations;


namespace ReservasTucson.Services.Support
{
    public static class Setup
    {
        /// <summary>
        /// Método Extensivo para la configuración de los Services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="=configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IClienteService, ClienteService>();
            services.AddTransient<IDetalleReservaService, DetalleReservaService>();
            services.AddTransient<IEstadoReservaService,EstadoReservaService>();
            services.AddTransient<IEventoService, EventoService>();
            services.AddTransient<IMesaService, MesaService>();
            services.AddTransient<IReservaMesaService, ReservaMesaService>();
            services.AddTransient<IReservaService, ReservaService>();
            services.AddTransient<ITipoReservaService, TipoReservaService>();
            services.AddTransient<IUsuarioService, UsuarioService>();
            services.AddTransient<ITipoUsuarioService, TipoUsuarioService>(); 

            return services;
        }
    }
}
