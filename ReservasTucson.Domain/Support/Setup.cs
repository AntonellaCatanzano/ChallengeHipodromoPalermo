
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using ReservasTucson.Domain.Profiles;

namespace ReservasTucson.Domain.Support
{
    public static class Setup
    {
        public static IServiceCollection AddEntitiesMappings(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(m =>
            {
                m.AddProfile(new ClienteProfile());
                m.AddProfile(new ReservaProfile());
                m.AddProfile(new DetalleReservaProfile());
                m.AddProfile(new EstadoReservaProfile());
                m.AddProfile(new ReservaMesaProfile());
                m.AddProfile(new TipoReservaProfile());
                m.AddProfile(new EventoProfile());
                m.AddProfile(new MesaProfile());
                m.AddProfile(new TipoUsuarioProfile());
                m.AddProfile(new UsuarioProfile());               
               
            });

            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);

            return services;
        }
    }
}
