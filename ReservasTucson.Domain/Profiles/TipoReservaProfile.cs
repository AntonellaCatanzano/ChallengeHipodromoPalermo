using AutoMapper;
using ReservasTucson.Domain.DTO;
using ReservasTucson.Domain.Entities;


namespace ReservasTucson.Domain.Profiles
{
    public class TipoReservaProfile : Profile
    {
        public TipoReservaProfile()
        {
            CreateMap<TipoReserva, TipoReservaDTO>()  
                .ForMember(destino => destino.Id, option => option.MapFrom(origen => origen.Id))
                .ForMember(destino => destino.Nombre, option => option.MapFrom(origen => origen.Nombre))
                .ForMember(destino => destino.RequiereSenia, option => option.MapFrom(origen => origen.RequiereSenia))
                .ForMember(destino => destino.MontoSenia, option => option.MapFrom(origen => origen.MontoSenia))
                .ForMember(destino => destino.TiempoPermanenciaMinutos, option => option.MapFrom(origen => origen.TiempoPermanenciaMinutos))
                .ForMember(destino => destino.PrioridadAsignacion, option => option.MapFrom(origen => origen.PrioridadAsignacion))
            .ReverseMap();
        }
    }
}
