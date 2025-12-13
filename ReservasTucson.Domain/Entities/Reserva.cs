using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasTucson.Domain.Entities
{
    [Table("Reservas", Schema = "dbo")]
    public class Reserva
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdReserva")]
        public int Id { get; set; }

        [DataType(DataType.Text)]
        [StringLength(50)]
        [Column("CodigoVip")]
        public string? CodigoVip { get; set; }

        [DataType(DataType.DateTime)]
        [Column("FechaHora")]
        public DateTime FechaHora { get; set; } = DateTime.Now;      

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column("CantidadPersonas")]
        public int CantidadPersonas { get; set; }        

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column("SeniaPagada")]        
        public bool SeniaPagada { get; set; }

        
        [DataType(DataType.Text)]
        [StringLength(100)]
        [Column("Observaciones")]
        public string? Observaciones { get; set; }        

        [DataType(DataType.DateTime)]
        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        [Column("FechaModificacion")]
        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;

        [DataType(DataType.Text)]
        [StringLength(300)]
        [Column("ObservacionCancelacion")]
        public string? ObservacionCancelacion { get; set; }


        #region Relaciones

        [ForeignKey("IdCliente")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column("IdCliente")]
        public int ClienteId { get; set; }           
        public Cliente Cliente { get; set; }

        [ForeignKey("IdTipoReserva")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column("IdTipoReserva")]
        public int TipoReservaId { get; set; }       
        public virtual TipoReserva TipoReserva { get; set; }
        
        public virtual DetalleReserva DetalleReserva { get; set; }  
        public virtual ICollection<ReservaMesa> ReservasMesas { get; set; } = new List<ReservaMesa>();

        [ForeignKey("IdEstadoReserva")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column("IdEstadoReserva")]
        public int EstadoReservaId { get; set; }
        public virtual EstadoReserva EstadoReserva { get; set; }

        [ForeignKey("IdUsuario")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column("IdUsuario")]
        public int UsuarioId { get; set; }  
        public virtual Usuario Usuario { get; set; }        

        #endregion
    }
}
