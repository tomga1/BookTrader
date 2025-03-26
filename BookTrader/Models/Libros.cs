using System.ComponentModel.DataAnnotations;

namespace BookTrader.Models
{
    public enum EstadoPublicacion
    {
        Pendiente,
        Aprobado,
        Rechazado
    }

    public class Libros : EntityBase
    {
        [Required]
        public string? Autor { get; set; }
        [Required]
        public string? Editorial { get; set; }
        public string? ISBN { get; set; }
        public int IdCategoria { get; set; } 
        public int IdCondicion { get; set; }
        [Required]
        public decimal Precio { get; set; }
        public string? ImagenUrl { get; set; }
        public string? MasInfo { get; set; }

        public EstadoPublicacion EstadoPublicacion { get; set; } = EstadoPublicacion.Pendiente;

        // Relaciones con otras tablas (opcional)
        public Categorias? Categoria { get; set; }
        public Condiciones? Condicion { get; set; }  // Relación con la tabla Condiciones
    }
}
