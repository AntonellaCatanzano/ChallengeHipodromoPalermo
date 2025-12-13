using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasTucson.Domain.DTO
{
    public class EstadoReservaDTO
    {        
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(200)]
        
        public string Descripcion { get; set; } // Pendiente, Confirmada, Cancelada, NoAsistió

        [Required(ErrorMessage = "El campo {0} es obligatorio")]        
        public bool EsFinal { get; set; }  // Cancelada, NoAsistió
    }
}
