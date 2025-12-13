using AutoMapper;
using ReservasTucson.Domain.DTO;
using ReservasTucson.Domain.Entities;

namespace ReservasTucson.Domain.Profiles
{
    public class ReservaMesaProfile : Profile
    {
        public ReservaMesaProfile()
        {
            CreateMap<ReservaMesa, ReservaMesaDTO>()
                .ForMember(destino => destino.Id, option => option.MapFrom(origen => origen.Id))
                .ForMember(destino => destino.Notas, option => option.MapFrom(origen => origen.Notas))                
                .ForMember(destino => destino.ReservaId, option => option.MapFrom(origen => origen.ReservaId))
                .ForMember(destino => destino.Reserva, option => option.MapFrom(origen => origen.Reserva))                
                .ForMember(destino => destino.MesaId, option => option.MapFrom(origen => origen.MesaId))                
            .ReverseMap();
        }
    }
}
