namespace ReservasTucson.Domain.DTO
{
    public class ReservaListItemDTO
    {
        public int Id { get; set; }
        public string ClienteNombre { get; set; } = null!;
        public string TipoReserva { get; set; } = null!;
        public string FechaHora { get; set; }
        public int CantidadPersonas { get; set; }
        public string Estado { get; set; } = null!;
    }
}
