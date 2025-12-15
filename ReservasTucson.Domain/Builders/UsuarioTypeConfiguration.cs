using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservasTucson.Domain.Entities;


namespace ReservasTucson.Domain.Builders
{
    public class UsuarioTypeConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(r => r.IdUsuario);

            builder.HasOne(er => er.TipoUsuario)
               .WithMany()
               .HasForeignKey(fk => fk.TipoUsuarioId)
               .HasPrincipalKey(pk => pk.Id);
        }
    }
}
