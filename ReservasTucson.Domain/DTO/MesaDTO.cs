using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasTucson.Domain.DTO
{
    public class MesaDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]

        public int Numero { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]

        public int Capacidad { get; set; }


        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(100)]

        public string Ubicacion { get; set; }

        [DataType(DataType.Text)]
        [StringLength(200)]        
        public string? Descripcion { get; set; }
        

        [Required(ErrorMessage = "El campo {0} es obligatorio")]

        public bool EsVip { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]

        public bool Activa { get; set; } = true;
       
    }
}
