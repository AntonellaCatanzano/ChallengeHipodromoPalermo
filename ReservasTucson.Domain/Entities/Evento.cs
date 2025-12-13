using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasTucson.Domain.Entities
{
    [Table("Eventos", Schema = "dbo")]
    public class Evento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdEvento")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(200)]
        [Column("Descripcion")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.DateTime)]
        [Column("FechaHoraInicio")]
        public DateTime FechaHoraInicio { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.DateTime)] 
        [Column("FechaHoraFin")]
        public DateTime FechaHoraFin { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column("CupoMaximo")]
        public int CupoMaximo { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column("Estado")]
        public bool Estado {  get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column("EsPrivado")]
        public bool EsPrivado { get; set; }

    }
}
