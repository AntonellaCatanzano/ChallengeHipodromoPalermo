using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservasTucson.Domain.DTO
{
    public class ReservaFilterDTO
    {
        public string? Nombre { get; set; }
        public string? Email { get; set; }
        public int? TipoReservaId { get; set; }
        public int? EstadoReservaId { get; set; }
        public string? Fecha { get; set; }
    }
}
