
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservasTucson.Domain.Entities;

namespace ReservasTucson.Domain.Builders
{
    public class EventoTypeConfiguration : IEntityTypeConfiguration<Evento>
    {
        public void Configure(EntityTypeBuilder<Evento> builder)
        {
            builder.HasKey(r => r.Id);
        }
    }
}
