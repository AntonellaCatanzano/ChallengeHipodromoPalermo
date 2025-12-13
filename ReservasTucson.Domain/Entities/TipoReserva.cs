using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasTucson.Domain.Entities
{
    [Table("TipoReservas", Schema = "dbo")]
    public class TipoReserva
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdTipoReserva")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(100)]
        [Column("Nombre")]
        public string Nombre { get; set; }

        
        [Column("RequiereSenia")]
        public bool? RequiereSenia { get; set; }

        [Column("MontoSenia")]
        public decimal? MontoSenia { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column("TiempoPermanenciaMinutos")]
        public int TiempoPermanenciaMinutos { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column("PrioridadAsignacion")]
        public int PrioridadAsignacion { get; set; }     
    }
}
