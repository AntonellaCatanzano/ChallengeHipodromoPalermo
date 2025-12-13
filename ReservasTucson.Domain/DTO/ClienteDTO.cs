using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasTucson.Domain.DTO
{
    public class ClienteDTO
    {
        
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        
        public bool EsPersonaFisica { get; set; }        

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(14)]
        
        public string Cuit { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(200)]
        
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(200)]        
        public string Apellido { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(150)]        
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(200)]        
        public string? RazonSocial { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(10)]
        
        public string? Telefono { get; set; }

        
        [DataType(DataType.Text)]
        [StringLength(300)]
       
        public string? Direccion { get; set; }

        [DataType(DataType.Text)]
        [StringLength(300)]
        
        public string? Observaciones { get; set; }
    }
}
