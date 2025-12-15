using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasTucson.Domain.Entities
{
    [Table("DetalleReservas", Schema = "dbo")]
    public class DetalleReserva
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdDetalleReserva")]
        public int Id { get; set; }                

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column("TraeTorta")]
        public bool TraeTorta { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column("EdadCumpleaniero")]
        public int EdadCumpleaniero { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column("Decoracion")]
        public bool? Decoracion { get; set; }

        [DataType(DataType.Text)]
        [StringLength(200)]
        [Column("CodigoVip")]
        public string? CodigoVip { get; set; }        

        [DataType(DataType.Text)]
        [StringLength(200)]
        [Column("ComentariosDecoracion")]
        public string? ComentariosDecoracion { get; set; }

        
        [DataType(DataType.Text)]
        [StringLength(200)]
        [Column("PaqueteContratado")]
        public string? PaqueteContratado { get; set; }

        #region Relaciones

        [ForeignKey("IdReserva")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column("IdReserva")]
        public int ReservaId { get; set; }           
        public virtual Reserva Reserva { get; set; }

        #endregion

    }
}
