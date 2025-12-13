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
                .HasPrincipalKey(pk => pk.Id);            

            builder.HasOne(tp => tp.TipoReserva)
                .WithMany()
                .HasForeignKey(fk => fk.TipoReservaId)
                .HasPrincipalKey(pk => pk.Id);

            builder.HasOne(er => er.EstadoReserva)
               .WithMany()
               .HasForeignKey(fk => fk.EstadoReservaId)
               .HasPrincipalKey(pk => pk.Id);

            builder.HasMany(rm => rm.ReservasMesas)
                .WithOne(r => r.Reserva)
                .HasPrincipalKey(pk => pk.Id);

            

        }
    }
}
