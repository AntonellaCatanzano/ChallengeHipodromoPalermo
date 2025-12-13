using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservasTucson.Domain.Entities;


namespace ReservasTucson.Domain.Builders
{
    public class MesaTypeConfiguration : IEntityTypeConfiguration<Mesa>
    {
        public void Configure(EntityTypeBuilder<Mesa> builder)
        {
            builder.HasKey(r => r.Id);
        }
    }
}
