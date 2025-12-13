using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasTucson.Domain.Entities
{
    [Table("TipoUsuarios", Schema = "dbo")]
    public class TipoUsuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdTipoUsuario")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(100)]
        [Column("Descripcion")]
        public string Descripcion { get; set; }   // "Cliente", "Empresa", "Recepcionista", etc.
    }
}
