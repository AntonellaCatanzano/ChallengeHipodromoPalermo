using ReservasTucson.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasTucson.Domain.DTO
{
    public class ReservaDTO
    {        
        public int Id { get; set; }

        [DataType(DataType.Text)]
        [StringLength(50)]
        public string? CodigoVip { get; set; }

        public string? Cuit{ get; set; }


        [DataType(DataType.DateTime)]
        
        public string FechaHora { get; set; } 

        [Required(ErrorMessage = "El campo {0} es obligatorio")]        
        public int CantidadPersonas { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        
        public bool SeniaPagada { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(100)]
        
        public string Observaciones { get; set; }

        [DataType(DataType.Text)]
        [StringLength(300)]        
        public string? ObservacionCancelacion { get; set; }       
        
        public string FechaCreacion { get; set; }        
        
        public string FechaModificacion { get; set; } 

        #region Relaciones

        
        [Required(ErrorMessage = "El campo {0} es obligatorio")]        
        public int ClienteId { get; set; }
        public ClienteDTO Cliente { get; set; }

        
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        
        public int TipoReservaId { get; set; }
        public TipoReservaDTO TipoReserva { get; set; }      
        

        public DetalleReservaDTO DetalleReserva { get; set; }
        public ICollection<ReservaMesaDTO> ReservasMesas { get; set; } = new List<ReservaMesaDTO>();

        
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        
        public int? UsuarioId { get; set; }
        public UsuarioDTO Usuario { get; set; }

        
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        
        public int EstadoReservaId { get; set; }
        public EstadoReservaDTO EstadoReserva { get; set; }

        #endregion
    }
}
