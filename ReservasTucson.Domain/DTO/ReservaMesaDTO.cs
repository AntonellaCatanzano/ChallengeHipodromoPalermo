using ReservasTucson.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservasTucson.Domain.DTO
{
    public class ReservaMesaDTO
    {        
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Text)]
        [StringLength(100)]
        
        public string Notas { get; set; }

        #region Relaciones

        
        [Required(ErrorMessage = "El campo {0} es obligatorio")]        
        public int ReservaId { get; set; }
        public ReservaDTO Reserva { get; set; }

        
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        
        public int MesaId { get; set; }
        public MesaDTO Mesa { get; set; }


        #endregion
    }
}
