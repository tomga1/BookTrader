namespace BookTrader.Models
{
    public class EntityBase
    {
        public int Id { get; set; }

        public string? Nombre { get; set; }

        public DateTime FechaPublicacion { get; set; }

        public DateTime FechaAgregado { get; set; }

        public string? Descripcion { get; set; }
        public int? IdUsuario { get; set; }

    }
}
