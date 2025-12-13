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
    public class TipoReservaTypeConfiguration : IEntityTypeConfiguration<TipoReserva>
    {
        public void Configure(EntityTypeBuilder<TipoReserva> builder)
        {
            builder.HasKey(r => r.Id);
        }
    }
}
