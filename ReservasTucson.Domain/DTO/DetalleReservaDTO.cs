using ReservasTucson.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasTucson.Domain.DTO
{
    public class DetalleReservaDTO
    {       
        public int Id { get; set; }        
        
        public bool? TraeTorta { get; set; }        
       
        public int? EdadCumpleaniero { get; set; }        
        
        public bool? Decoracion { get; set; }        

        [DataType(DataType.Text)]
        [StringLength(200)]       
        public string? CodigoVip { get; set; }

        
        [DataType(DataType.Text)]
        [StringLength(200)]
        public string? ComentariosDecoracion { get; set; }
        
        [DataType(DataType.Text)]
        [StringLength(200)]
        
        public string? PaqueteContratado { get; set; }

        #region Relaciones
        
        [Required(ErrorMessage = "El campo {0} es obligatorio")]        
        public int ReservaId { get; set; }     
        
        public int? MesaId { get; set; }

        public MesaDTO? Mesa { get; set; }

        #endregion
    }
}
