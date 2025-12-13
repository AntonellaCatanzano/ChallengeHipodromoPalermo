using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasTucson.Domain.DTO
{
    public class TipoReservaDTO
    {       
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(100)]
        
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        
        public bool RequiereSenia { get; set; }
        public decimal? MontoSenia { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        
        public int TiempoPermanenciaMinutos { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]       
        public int PrioridadAsignacion { get; set; }
    }
}
