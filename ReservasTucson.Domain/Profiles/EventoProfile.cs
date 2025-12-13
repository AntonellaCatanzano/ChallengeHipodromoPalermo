using AutoMapper;
using ReservasTucson.Domain.DTO;
using ReservasTucson.Domain.Entities;
using System.Globalization;

namespace ReservasTucson.Domain.Profiles
{
    public class EventoProfile : Profile
    {
        public EventoProfile()
        {
            CreateMap<Evento, EventoDTO>()
                .ForMember(destino => destino.Id, option => option.MapFrom(origen => origen.Id))
                .ForMember(destino => destino.Descripcion, option => option.MapFrom(origen => origen.Descripcion))
                .ForMember(destino => destino.FechaHoraInicio,
                    opt => opt.MapFrom(origen => origen.FechaHoraInicio.ToString("dd/MM/yyyy HH:mm:ss")))
                .ForMember(destino => destino.FechaHoraFin,
                        opt => opt.MapFrom(origen => origen.FechaHoraFin.ToString("dd/MM/yyyy HH:mm:ss")))
                .ForMember(destino => destino.CupoMaximo, option => option.MapFrom(origen => origen.CupoMaximo))
                .ForMember(destino => destino.Estado, option => option.MapFrom(origen => origen.Estado))
                .ForMember(destino => destino.EsPrivado, option => option.MapFrom(origen => origen.EsPrivado))
            .ReverseMap()
            .ForMember(dest => dest.FechaHoraInicio,
               opt => opt.MapFrom(src => DateTime.ParseExact(
                   src.FechaHoraInicio,
                   "dd/MM/yyyy HH:mm:ss",
               CultureInfo.InvariantCulture)))
            .ForMember(dest => dest.FechaHoraFin,
                opt => opt.MapFrom(src => DateTime.ParseExact(
                    src.FechaHoraFin,
                    "dd/MM/yyyy HH:mm:ss",
                    CultureInfo.InvariantCulture)));
        }
    }
}
