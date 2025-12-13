using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasTucson.Domain.Entities
{
    [Table("Usuarios", Schema = "dbo")]
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdUsuario")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(100)]
        [Column("Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(100)]
        [Column("Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(100)]
        [Column("PasswordHash")]
        public string PasswordHash { get; set; }

        #region Relaciones
        [ForeignKey("IdTipoUsuario")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column("IdTipoUsuario")]
        public int TipoUsuarioId { get; set; }
        public virtual TipoUsuario TipoUsuario { get; set; }
        #endregion

    }

}
