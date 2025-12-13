
using AutoMapper;
using ReservasTucson.Domain.DTO;
using ReservasTucson.Domain.Entities;

namespace ReservasTucson.Domain.Profiles
{
    public class TipoUsuarioProfile : Profile
    {
        public TipoUsuarioProfile()
        {
            CreateMap<TipoUsuario, TipoUsuarioDTO>()
                .ForMember(destino => destino.Id, option => option.MapFrom(origen => origen.Id))
                .ForMember(destino => destino.Descripcion, option => option.MapFrom(origen => origen.Descripcion))                
            .ReverseMap();
        }
    }
}
