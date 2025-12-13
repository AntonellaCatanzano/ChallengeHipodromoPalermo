using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasTucson.Domain.Entities
{
    [Table("ReservasMesas", Schema = "dbo")]
    public class ReservaMesa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdReservaMesa")]
        public int Id { get; set; }       

        
        [DataType(DataType.Text)]
        [StringLength(100)]
        [Column("Notas")]
        public string? Notas { get; set; }

        #region Relaciones

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [ForeignKey("IdReserva")]        
        [Column("IdReserva")]
        public int ReservaId { get; set; }
        public virtual Reserva Reserva { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [ForeignKey("IdMesa")]        
        [Column("IdMesa")]
        public int MesaId { get; set; }
        public virtual Mesa Mesa { get; set; } 
        

        #endregion
    }
}
