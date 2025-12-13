using ReservasTucson.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasTucson.Domain.DTO
{
    public class UsuarioDTO
    {        
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(100)]
        
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(100)]
        
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(100)]
        
        public string PasswordHash { get; set; }

        #region Relaciones
       
        [Required(ErrorMessage = "El campo {0} es obligatorio")]        
        public int TipoUsuarioId { get; set; }
        public TipoUsuarioDTO TipoUsuario { get; set; }

        #endregion
    }
}
