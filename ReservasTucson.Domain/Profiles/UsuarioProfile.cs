
using AutoMapper;
using ReservasTucson.Domain.DTO;
using ReservasTucson.Domain.Entities;

namespace ReservasTucson.Domain.Profiles
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<Usuario, UsuarioDTO>()  
                .ForMember(destino => destino.IdUsuario, option => option.MapFrom(origen => origen.IdUsuario))
                .ForMember(destino => destino.Nombre, option => option.MapFrom(origen => origen.Nombre))
                .ForMember(destino => destino.Email, option => option.MapFrom(origen => origen.Email))
                .ForMember(destino => destino.PasswordHash, option => option.MapFrom(origen => origen.PasswordHash))
                .ForMember(destino => destino.TipoUsuarioId, option => option.MapFrom(origen => origen.TipoUsuarioId))
                .ForMember(destino => destino.TipoUsuario, option => option.MapFrom(origen => origen.TipoUsuario))
            .ReverseMap();
        }
    }
}
