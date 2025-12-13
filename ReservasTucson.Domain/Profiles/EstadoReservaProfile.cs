using AutoMapper;
using ReservasTucson.Domain.DTO;
using ReservasTucson.Domain.Entities;

namespace ReservasTucson.Domain.Profiles
{
    public class EstadoReservaProfile : Profile
    {
        public EstadoReservaProfile()
        {
            CreateMap<EstadoReserva, EstadoReservaDTO>()
                .ForMember(destino => destino.Id, option => option.MapFrom(origen => origen.Id))
                .ForMember(destino => destino.Descripcion, option => option.MapFrom(origen => origen.Descripcion))
                .ForMember(destino => destino.EsFinal, option => option.MapFrom(origen => origen.EsFinal))                
            .ReverseMap();
        }
    }
}
