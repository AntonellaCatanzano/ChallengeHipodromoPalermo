using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservasTucson.Domain.Entities;


namespace ReservasTucson.Domain.Builders
{
    public class EstadoReservaTypeConfiguration : IEntityTypeConfiguration<EstadoReserva>
    {
        public void Configure(EntityTypeBuilder<EstadoReserva> builder)
        {
            builder.HasKey(r => r.Id);           
        }
    }
}
