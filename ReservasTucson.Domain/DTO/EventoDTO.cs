using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasTucson.Domain.DTO
{
    public class EventoDTO
    {        
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(200)]
       
        public string Descripcion { get; set; }        

        public string FechaHoraInicio { get; set; }        
        
        public string FechaHoraFin { get; set; } 

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
       
        public int CupoMaximo { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        
        public bool Estado { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        
        public bool EsPrivado { get; set; }
    }
}
