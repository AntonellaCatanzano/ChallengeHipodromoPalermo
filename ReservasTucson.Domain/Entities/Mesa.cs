using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasTucson.Domain.Entities
{
    [Table("Mesas", Schema = "dbo")]
    public class Mesa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdMesa")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column("Numero")]
        public int Numero { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column("Capacidad")]
        public int Capacidad { get; set; }

        
        [DataType(DataType.Text)]
        [StringLength(200)]
        [Column("Descripcion")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(100)]
        [Column("Ubicacion")]
        public string Ubicacion { get; set; }         

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column("EsVip")]
        public bool EsVip { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column("Activa")]
        public bool Activa { get; set; } = true;
        
    }
}
