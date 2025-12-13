using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservasTucson.Domain.Entities;


namespace ReservasTucson.Domain.Builders
{
    public class ReservaMesaTypeConfiguration : IEntityTypeConfiguration<ReservaMesa>
    {
        public void Configure(EntityTypeBuilder<ReservaMesa> builder)
        {
            builder.HasKey(r => r.Id);

            builder.HasOne(rm => rm.Reserva)
            .WithMany(r => r.ReservasMesas)
            .HasForeignKey(rm => rm.ReservaId);

            builder.HasOne(rm => rm.Mesa)
                .WithMany()
                .HasForeignKey(rm => rm.MesaId);

        }
    }
}
