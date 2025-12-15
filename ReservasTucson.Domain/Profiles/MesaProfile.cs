using AutoMapper;
using ReservasTucson.Domain.DTO;
using ReservasTucson.Domain.Entities;


namespace ReservasTucson.Domain.Profiles
{
    public class MesaProfile : Profile
    {
        public MesaProfile()
        {
            CreateMap<Mesa, MesaDTO>()  
                .ForMember(destino => destino.Id, option => option.MapFrom(origen => origen.Id))
                .ForMember(destino => destino.Numero, option => option.MapFrom(origen => origen.Numero))
                .ForMember(destino => destino.Capacidad, option => option.MapFrom(origen => origen.Capacidad))
                .ForMember(destino => destino.Ubicacion, option => option.MapFrom(origen => origen.Ubicacion))
                .ForMember(destino => destino.Descripcion, option => option.MapFrom(origen => origen.Descripcion))
                .ForMember(destino => destino.EsVip, option => option.MapFrom(origen => origen.EsVip))
                .ForMember(destino => destino.Activa, option => option.MapFrom(origen => origen.Activa))
           .ReverseMap();
        }
    }
}
