namespace BookTrader.DTOs
{
    public class InsertLibroDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string Editorial { get; set; } = string.Empty;
        public string? ISBN { get; set; }
        public int IdCategoria { get; set; }
        public int IdCondicion { get; set; }
        public decimal Precio { get; set; }
        public string? ImagenUrl { get; set; }
        public string? MasInfo { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public string? Descripcion { get; set; }
        public int? IdUsuario { get; set; }

    }
}
