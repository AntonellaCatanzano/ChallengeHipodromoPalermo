
namespace ReservasTucson.Domain.DTO
{
    public class AsignarMesasRequestDTO
    {
        public int ReservaId { get; set; }
        public List<int> MesaIds { get; set; } = new();
    }
}
