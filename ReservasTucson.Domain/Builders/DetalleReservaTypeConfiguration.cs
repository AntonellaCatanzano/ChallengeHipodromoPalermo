using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservasTucson.Domain.Entities;


namespace ReservasTucson.Domain.Builders
{
    public class DetalleReservaTypeConfiguration : IEntityTypeConfiguration<DetalleReserva>
    {
        public void Configure(EntityTypeBuilder<DetalleReserva> builder)
        {
            builder.HasKey(r => r.Id);

            builder.HasOne(d => d.Reserva)
               .WithOne(r => r.DetalleReserva)
               .HasForeignKey<DetalleReserva>(d => d.ReservaId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
