using AutoMapper;
using ReservasTucson.Domain.DTO;
using ReservasTucson.Domain.Entities;

namespace ReservasTucson.Domain.Profiles
{
    public class DetalleReservaProfile : Profile
    {
        public DetalleReservaProfile()
        {
            CreateMap<DetalleReserva, DetalleReservaDTO>()  
                .ForMember(destino => destino.Id, option => option.MapFrom(origen => origen.Id))
                .ForMember(destino => destino.TraeTorta, option => option.MapFrom(origen => origen.TraeTorta))
                .ForMember(destino => destino.EdadCumpleaniero, option => option.MapFrom(origen => origen.EdadCumpleaniero))
                .ForMember(destino => destino.Decoracion, option => option.MapFrom(origen => origen.Decoracion))
                .ForMember(destino => destino.ComentariosDecoracion, option => option.MapFrom(origen => origen.ComentariosDecoracion))
                .ForMember(destino => destino.PaqueteContratado, option => option.MapFrom(origen => origen.PaqueteContratado))               
                .ForMember(destino => destino.ReservaId, option => option.MapFrom(origen => origen.ReservaId))
                .ForMember(destino => destino.MesaId, option => option.MapFrom(origen => origen.MesaId))
                .ForMember(destino => destino.Mesa, option => option.Ignore())
                
            .ReverseMap();
        }
    }
}
