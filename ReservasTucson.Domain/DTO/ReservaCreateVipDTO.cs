using System.ComponentModel.DataAnnotations;

namespace ReservasTucson.Domain.DTO
{
    public class ReservaCreateVipDTO
    {        
        public int? IdCliente { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El email ingresado no es válido.")]
        public string Email { get; set; }

        public string Telefono { get; set; }

        [Required(ErrorMessage = "La fecha y hora son obligatorias.")]
        public string FechaHora { get; set; }

        [Required(ErrorMessage = "La cantidad de personas es obligatoria.")]
        public int CantidadPersonas { get; set; }

        [Required(ErrorMessage = "El código VIP es obligatorio.")]
        public string CodigoVip { get; set; }

        [Required(ErrorMessage = "El cuit es obligatorio.")]
        public string Cuit { get; set; }

        public int? MesaPreferidaId { get; set; }
        public string? Observaciones { get; set; }
    }
}
