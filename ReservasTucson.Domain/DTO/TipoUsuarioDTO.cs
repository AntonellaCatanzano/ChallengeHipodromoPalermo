using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasTucson.Domain.DTO
{
    public class TipoUsuarioDTO
    {        
        
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(100)]        
        public string Descripcion { get; set; }   
    }
}
