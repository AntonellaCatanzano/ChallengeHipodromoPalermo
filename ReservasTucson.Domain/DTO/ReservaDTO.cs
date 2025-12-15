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

        public string? Cuit { get; set; }

        [Required]
        public string FechaHora { get; set; } = null!;  // Fecha como string

        [Required]
        public int CantidadPersonas { get; set; }

        public bool SeniaPagada { get; set; }

        [Required]
        [StringLength(100)]
        public string Observaciones { get; set; } = null!;

        [StringLength(300)]
        public string? ObservacionCancelacion { get; set; }

        public string FechaCreacion { get; set; } = null!;  // Fecha como string
        public string FechaModificacion { get; set; } = null!;  // Fecha como string

        #region Relaciones
        public int? ClienteId { get; set; }
        public ClienteDTO? Cliente { get; set; }

        public int? TipoReservaId { get; set; }
        public TipoReservaDTO? TipoReserva { get; set; }

        public DetalleReservaDTO? DetalleReserva { get; set; }
        public ICollection<ReservaMesaDTO>? ReservasMesas { get; set; }

        public int? UsuarioId { get; set; }
        public UsuarioDTO? Usuario { get; set; }

        [Required]
        public int EstadoReservaId { get; set; }
        public EstadoReservaDTO? EstadoReserva { get; set; }

        #endregion

    }
}
