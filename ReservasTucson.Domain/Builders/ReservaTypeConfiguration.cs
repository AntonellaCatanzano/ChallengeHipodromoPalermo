using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservasTucson.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservasTucson.Domain.Builders
{
    public class ReservaTypeConfiguration : IEntityTypeConfiguration<Reserva>
    {
        public void Configure(EntityTypeBuilder<Reserva> builder)
        {
            builder.HasKey(r => r.Id);

            builder.HasOne(c => c.Cliente)
                .WithMany()
                .HasForeignKey(fk => fk.ClienteId)
                .HasPrincipalKey(pk => pk.Id)
                .OnDelete(DeleteBehavior.Cascade); ;

            builder.HasOne(tp => tp.TipoReserva)
                .WithMany()
                .HasForeignKey(fk => fk.TipoReservaId)
                .OnDelete(DeleteBehavior.Cascade); 

            builder.HasOne(er => er.EstadoReserva)
               .WithMany()
               .HasForeignKey(fk => fk.EstadoReservaId)
               .OnDelete(DeleteBehavior.Cascade); 
               

            builder.HasMany(r => r.ReservasMesas)
               .WithOne(rm => rm.Reserva)
               .HasForeignKey(rm => rm.ReservaId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Usuario)
                .WithMany()
                .HasForeignKey(r => r.UsuarioId)
                .HasPrincipalKey(u => u.IdUsuario);


            builder.HasOne(r => r.DetalleReserva)
               .WithOne(d => d.Reserva)
               .HasForeignKey<DetalleReserva>(d => d.ReservaId)
               .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
