namespace BookTrader.Models
{
    public class IdiomasEntity
    {
        public int Id { get; set; }

        public string? Nombre { get; set; }

        public DateTime FechaAgregado { get; set; } = DateTime.Now;

        public string? Descripcion { get; set; }
        public string? IdUsuario { get; set; }
        public bool Estado { get; set; }

        public virtual ICollection<Libros>? Libros { get; set; }

    }
}
