namespace BookTrader.Models
{
    public class Libros : EntityBase
    {
        public string? Autor { get; set; }
        public string? Editorial { get; set; }
        public string? ISBN { get; set; }
        public int IdCategoria { get; set; } 
        public int IdCondicion { get; set; }  
        public decimal Precio { get; set; }
        public string? ImagenUrl { get; set; }

        // Relaciones con otras tablas (opcional)
        public Categorias? Categoria { get; set; }
        public Condiciones? Condicion { get; set; }  // Relación con la tabla Condiciones
    }
}
