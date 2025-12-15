using AutoMapper;
using ReservasTucson.Domain.DTO;
using ReservasTucson.Domain.Entities;
using System.Globalization;

namespace ReservasTucson.Domain.Profiles
{
    public class ReservaProfile : Profile
    {
        public ReservaProfile()
        {
            
            CreateMap<Reserva, ReservaDTO>()
                .ForMember(dest => dest.FechaHora,
                           opt => opt.MapFrom(src => src.FechaHora.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.FechaCreacion,
                           opt => opt.MapFrom(src => src.FechaCreacion.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.FechaModificacion,
                           opt => opt.MapFrom(src => src.FechaModificacion.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => src.Cliente))
                .ForMember(dest => dest.TipoReserva, opt => opt.MapFrom(src => src.TipoReserva))
                .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.Usuario))
                .ForMember(dest => dest.EstadoReserva, opt => opt.MapFrom(src => src.EstadoReserva))
                .ForMember(dest => dest.ReservasMesas, opt => opt.MapFrom(src => src.ReservasMesas));

            // Map entidad Reserva a ReservaDetailDTO
            CreateMap<Reserva, ReservaDetailDTO>()
                .IncludeBase<Reserva, ReservaDTO>()
                .ForMember(dest => dest.DetalleReserva, opt => opt.MapFrom(src => src.DetalleReserva))
                .ForMember(dest => dest.ReservasMesas, opt => opt.MapFrom(src => src.ReservasMesas));
            
            CreateMap<DetalleReserva, DetalleReservaDTO>();
            
            CreateMap<ReservaMesa, ReservaMesaDTO>()
                .ForMember(dest => dest.Mesa, opt => opt.MapFrom(src => src.Mesa));

            // Map de DTO de creación estándar a entidad Reserva
            CreateMap<ReservaCreateStandardDTO, Reserva>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ClienteId, opt => opt.Ignore())
                .ForMember(dest => dest.Cliente, opt => opt.Ignore())
                .ForMember(dest => dest.TipoReservaId, opt => opt.Ignore())
                .ForMember(dest => dest.EstadoReservaId, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioId, opt => opt.Ignore())
                .ForMember(dest => dest.ReservasMesas, opt => opt.Ignore())
                .ForMember(dest => dest.FechaHora, opt => opt.Ignore());

            // Map de DTO de creación a DTO de salida
            CreateMap<ReservaCreateStandardDTO, ReservaDTO>()
                .ForMember(dest => dest.FechaHora, opt => opt.Ignore()) // Se asigna en service
                .ForMember(dest => dest.Cliente, opt => opt.MapFrom(MapClienteFromCreateDTO))
                .ForMember(dest => dest.TipoReservaId, opt => opt.Ignore())
                .ForMember(dest => dest.EstadoReservaId, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioId, opt => opt.Ignore())
                .ForMember(dest => dest.ReservasMesas, opt => opt.Ignore());

            // Map de DTO de creación VIP a entidad Reserva
            CreateMap<ReservaCreateVipDTO, Reserva>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ClienteId, opt => opt.Ignore())
                .ForMember(dest => dest.Cliente, opt => opt.Ignore())
                .ForMember(dest => dest.TipoReservaId, opt => opt.Ignore())
                .ForMember(dest => dest.EstadoReservaId, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioId, opt => opt.Ignore())
                .ForMember(dest => dest.ReservasMesas, opt => opt.Ignore())
                .ForMember(dest => dest.FechaHora, opt => opt.Ignore()) // Se asigna en el service
                .ForMember(dest => dest.DetalleReserva, opt => opt.MapFrom(src => new DetalleReserva { CodigoVip = src.CodigoVip }));

            // Map de DTO de creación Cumpleaños a entidad Reserva
            CreateMap<ReservaCreateCumpleDTO, Reserva>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ClienteId, opt => opt.Ignore())
                .ForMember(dest => dest.Cliente, opt => opt.Ignore())
                .ForMember(dest => dest.TipoReservaId, opt => opt.Ignore())
                .ForMember(dest => dest.EstadoReservaId, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioId, opt => opt.Ignore())
                .ForMember(dest => dest.ReservasMesas, opt => opt.Ignore())
                .ForMember(dest => dest.FechaHora, opt => opt.Ignore()) // Se asigna en el service
                .ForMember(dest => dest.DetalleReserva, opt => opt.MapFrom(src => new DetalleReserva
                {
                    EdadCumpleaniero = src.EdadCumpleaniero,
                    TraeTorta = src.TraeTorta
                }));

            // Opcional: Mapear también a DTO de salida si querés usar Map de creación -> DTO
            CreateMap<ReservaCreateVipDTO, ReservaDTO>()
                .ForMember(dest => dest.FechaHora, opt => opt.Ignore())
                .ForMember(dest => dest.Cliente, opt => opt.MapFrom(MapClienteFromCreateDTO))
                .ForMember(dest => dest.TipoReservaId, opt => opt.Ignore())
                .ForMember(dest => dest.EstadoReservaId, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioId, opt => opt.Ignore())
                .ForMember(dest => dest.ReservasMesas, opt => opt.Ignore());

            CreateMap<ReservaCreateCumpleDTO, ReservaDTO>()
                .ForMember(dest => dest.FechaHora, opt => opt.Ignore())
                .ForMember(dest => dest.Cliente, opt => opt.MapFrom(MapClienteFromCreateDTO))
                .ForMember(dest => dest.TipoReservaId, opt => opt.Ignore())
                .ForMember(dest => dest.EstadoReservaId, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioId, opt => opt.Ignore())
                .ForMember(dest => dest.ReservasMesas, opt => opt.Ignore());

            CreateMap<DetalleReserva, DetalleReservaDTO>();
            CreateMap<ReservaMesa, ReservaMesaDTO>();
        }

        // Método para mapear Cliente desde DTO de creación
        private static Cliente MapClienteFromCreateDTO(ReservaCreateStandardDTO src, ReservaDTO dest)
        {
            if (src.IdCliente.HasValue)
                return new Cliente { Id = src.IdCliente.Value };

            return new Cliente
            {
                Nombre = src.Nombre.Trim(),
                Apellido = src.Apellido.Trim(),
                Email = src.Email.Trim(),
                Telefono = src.Telefono?.Trim(),
                Cuit = src.Cuit?.Trim()
            };
        }

        private static Cliente MapClienteFromCreateDTO(ReservaCreateVipDTO src, ReservaDTO dest)
        {
            if (src.IdCliente.HasValue)
                return new Cliente { Id = src.IdCliente.Value };

            return new Cliente
            {
                Nombre = src.Nombre.Trim(),
                Apellido = src.Apellido.Trim(),
                Email = src.Email.Trim(),
                Telefono = src.Telefono?.Trim(),
                Cuit = src.Cuit?.Trim()
            };
        }

        private static Cliente MapClienteFromCreateDTO(ReservaCreateCumpleDTO src, ReservaDTO dest)
        {
            if (src.IdCliente.HasValue)
                return new Cliente { Id = src.IdCliente.Value };

            return new Cliente
            {
                Nombre = src.Nombre.Trim(),
                Apellido = src.Apellido.Trim(),
                Email = src.Email.Trim(),
                Telefono = src.Telefono?.Trim(),
                Cuit = src.Cuit?.Trim()
            };
        }
        /*CreateMap<ReservaDTO, Reserva>()

        .ForMember(dest => dest.FechaHora,
                   opt => opt.MapFrom(src => DateTime.Parse(src.FechaHora)))


        .ForMember(dest => dest.FechaCreacion,
                   opt => opt.MapFrom(src => DateTime.Parse(src.FechaCreacion)))
        .ForMember(dest => dest.FechaModificacion,
                   opt => opt.MapFrom(src => DateTime.Parse(src.FechaModificacion)))
        .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UsuarioId))

        .ForMember(dest => dest.Cliente, opt => opt.Ignore())
        .ForMember(dest => dest.TipoReserva, opt => opt.Ignore())
        .ForMember(dest => dest.Usuario, opt => opt.Ignore())
        .ForMember(dest => dest.EstadoReserva, opt => opt.Ignore())            
        .ForMember(dest => dest.ReservasMesas, opt => opt.Ignore())


        .ForMember(dest => dest.DetalleReserva,
                   opt => opt.MapFrom(src => src.DetalleReserva))

        .ReverseMap()

        .ForMember(dest => dest.FechaHora,
                   opt => opt.MapFrom(src => src.FechaHora.ToString("yyyy-MM-dd HH:mm:ss")))
        .ForMember(dest => dest.FechaCreacion,
                   opt => opt.MapFrom(src => src.FechaCreacion.ToString("yyyy-MM-dd HH:mm:ss")))
        .ForMember(dest => dest.FechaModificacion,
                   opt => opt.MapFrom(src => src.FechaModificacion.ToString("yyyy-MM-dd HH:mm:ss")))

        // Evita referencia circular
        .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => src.Cliente))
        .ForMember(dest => dest.TipoReserva, opt => opt.MapFrom(src => src.TipoReserva))
        .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.Usuario))
        .ForMember(dest => dest.EstadoReserva, opt => opt.MapFrom(src => src.EstadoReserva))            
        .ForMember(dest => dest.ReservasMesas,
                   opt => opt.MapFrom(src => src.ReservasMesas));



        CreateMap<ReservaCreateStandardDTO, Reserva>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ClienteId, opt => opt.Ignore())
            .ForMember(dest => dest.Cliente, opt => opt.Ignore())
            .ForMember(dest => dest.DetalleReserva, opt => opt.Ignore())  
            .ForMember(dest => dest.TipoReservaId, opt => opt.Ignore())
            .ForMember(dest => dest.EstadoReservaId, opt => opt.Ignore())
            .ForMember(dest => dest.UsuarioId, opt => opt.Ignore())
            .ForMember(dest => dest.ReservasMesas, opt => opt.Ignore())  
            .ForMember(dest => dest.FechaHora, opt => opt.Ignore());      



        CreateMap<ReservaCreateVipDTO, Reserva>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ClienteId, opt => opt.Ignore())
            .ForMember(dest => dest.Cliente, opt => opt.Ignore())
            .ForMember(dest => dest.DetalleReserva, opt => opt.Ignore())   
            .ForMember(dest => dest.TipoReservaId, opt => opt.Ignore())
            .ForMember(dest => dest.EstadoReservaId, opt => opt.Ignore())
            .ForMember(dest => dest.UsuarioId, opt => opt.Ignore())
            .ForMember(dest => dest.ReservasMesas, opt => opt.Ignore())
            .ForMember(dest => dest.FechaHora, opt => opt.Ignore());     



        CreateMap<ReservaCreateCumpleDTO, Reserva>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ClienteId, opt => opt.Ignore())
            .ForMember(dest => dest.Cliente, opt => opt.Ignore())
            .ForMember(dest => dest.DetalleReserva, opt => opt.Ignore())   
            .ForMember(dest => dest.TipoReservaId, opt => opt.Ignore())
            .ForMember(dest => dest.EstadoReservaId, opt => opt.Ignore())
            .ForMember(dest => dest.UsuarioId, opt => opt.Ignore())
            .ForMember(dest => dest.ReservasMesas, opt => opt.Ignore())
            .ForMember(dest => dest.FechaHora, opt => opt.Ignore());




        CreateMap<ReservaCreateStandardDTO, ReservaDTO>()
            .ForMember(dest => dest.FechaHora, opt => opt.Ignore())
            .ForMember(dest => dest.DetalleReserva, opt => opt.Ignore())
            .ForMember(dest => dest.Cliente, opt => opt.MapFrom(MapClienteStandard));


        CreateMap<ReservaCreateVipDTO, ReservaDTO>()
            .ForMember(dest => dest.CodigoVip, opt => opt.MapFrom(src => src.CodigoVip))
            .ForMember(dest => dest.Cuit, opt => opt.MapFrom(src => src.Cuit))
            .ForMember(dest => dest.FechaHora, opt => opt.Ignore())
            .ForMember(dest => dest.DetalleReserva, opt => opt.Ignore())
            .ForMember(dest => dest.Cliente, opt => opt.MapFrom(MapClienteVip));


        CreateMap<ReservaCreateCumpleDTO, ReservaDTO>()
            .ForMember(dest => dest.Cuit, opt => opt.MapFrom(src => src.Cuit))
            .ForMember(dest => dest.FechaHora, opt => opt.Ignore())
            .ForMember(dest => dest.DetalleReserva, opt => opt.Ignore())
            .ForMember(dest => dest.Cliente, opt => opt.MapFrom(MapClienteCumple));


        CreateMap<ReservaDTO, ReservaListItemDTO>()
            .ForMember(dest => dest.ClienteNombre,
                opt => opt.MapFrom(src => src.Cliente.Nombre + " " + src.Cliente.Apellido))
            .ForMember(dest => dest.TipoReserva,
                opt => opt.MapFrom(src => src.TipoReserva.Nombre))
            .ForMember(dest => dest.Estado,
                opt => opt.MapFrom(src => src.EstadoReserva.Descripcion));


        CreateMap<DetalleReservaDTO, DetalleReserva>()
            .ReverseMap();

        CreateMap<Reserva, ReservaDetailDTO>()
            .IncludeBase<Reserva, ReservaDTO>();
    }        

    private static Cliente MapClienteStandard(ReservaCreateStandardDTO src, ReservaDTO destino)
    {
        if (src.IdCliente.HasValue)
            return new Cliente { Id = src.IdCliente.Value };

        return new Cliente
        {
            Nombre = src.Nombre.Trim(),
            Apellido = src.Apellido.Trim(),
            Email = src.Email.Trim(),
            Telefono = src.Telefono.Trim(),
            Cuit = src.Cuit.Trim()
        };
    }

    private static Cliente MapClienteVip(ReservaCreateVipDTO src, ReservaDTO destino)
    {
        if (src.IdCliente.HasValue)
            return new Cliente { Id = src.IdCliente.Value };

        return new Cliente
        {
            Nombre = src.Nombre.Trim(),
            Apellido = src.Apellido.Trim(),
            Email = src.Email.Trim(),
            Telefono = src.Telefono.Trim(),
            Cuit = src.Cuit.Trim()
        };
    }

    private static Cliente MapClienteCumple(ReservaCreateCumpleDTO src, ReservaDTO destino)
    {
        if (src.IdCliente.HasValue)
            return new Cliente { Id = src.IdCliente.Value };

        return new Cliente
        {
            Nombre = src.Nombre.Trim(),
            Apellido = src.Apellido.Trim(),
            Email = src.Email.Trim(),
            Telefono = src.Telefono?.Trim(),
            Cuit = src.Cuit.Trim()
        };
    } */
    }
}

