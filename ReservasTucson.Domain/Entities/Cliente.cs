using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasTucson.Domain.Entities
{
    [Table("Clientes", Schema = "dbo")]
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdCliente")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column("EsPersonaFisica")]
        public bool EsPersonaFisica { get; set; }        
        
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(14)]
        [Column("Cuit")]
        public string Cuit { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(200)]
        [Column("Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(200)]
        [Column("Apellido")]
        public string Apellido { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(150)]
        [Column("Email")]
        public string Email { get; set; }


        [DataType(DataType.Text)]
        [StringLength(200)]
        [Column("RazonSocial")]
        public string? RazonSocial { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(10)]
        [Column("Telefono")]        
        public string Telefono { get; set; }

        
        [DataType(DataType.Text)]
        [StringLength(300)]
        [Column("Direccion")]
        public string? Direccion {  get; set; }

        [DataType(DataType.Text)]
        [StringLength(300)]
        [Column("observaciones")]
        public string? Observaciones { get; set; }
    }
}
