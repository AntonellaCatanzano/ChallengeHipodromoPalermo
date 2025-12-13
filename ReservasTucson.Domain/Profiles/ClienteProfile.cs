using AutoMapper;
using ReservasTucson.Domain.DTO;
using ReservasTucson.Domain.Entities;

namespace ReservasTucson.Domain.Profiles
{
    public class ClienteProfile : Profile
    {
        public ClienteProfile()
        {
            CreateMap<Cliente, ClienteDTO>()  
                .ForMember(destino => destino.Id, option => option.MapFrom(origen => origen.Id))
                .ForMember(destino => destino.EsPersonaFisica, option => option.MapFrom(origen => origen.EsPersonaFisica))                
                .ForMember(destino => destino.Cuit, option => option.MapFrom(origen => origen.Cuit))
                .ForMember(destino => destino.Nombre, option => option.MapFrom(origen => origen.Nombre))
                .ForMember(destino => destino.Apellido, option => option.MapFrom(origen => origen.Apellido))
                .ForMember(destino => destino.Email, option => option.MapFrom(origen => origen.Email))
                .ForMember(destino => destino.RazonSocial, option => option.MapFrom(origen => origen.RazonSocial))
                .ForMember(destino => destino.Telefono, option => option.MapFrom(origen => origen.Telefono))
                .ForMember(destino => destino.Direccion, option => option.MapFrom(origen => origen.Direccion))
                .ForMember(destino => destino.Observaciones, option => option.MapFrom(origen => origen.Observaciones))

            .ReverseMap();
        }
    }
}
