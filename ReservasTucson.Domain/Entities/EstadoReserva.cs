using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasTucson.Domain.Entities
{
    [Table("EstadosReservas", Schema = "dbo")]
    public class EstadoReserva
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdEstadoReserva")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(200)]
        [Column("Descripcion")]
        public string Descripcion { get; set; } 

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column("EsFinal")]
        public bool EsFinal { get; set; }  
    }
}
