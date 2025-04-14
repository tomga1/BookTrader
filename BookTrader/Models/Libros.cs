using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        public int CategoriaId { get; set; } 
        public int CondicionId { get; set; }

        [Required]
        public decimal Precio { get; set; }
        public string? ImagenUrl { get; set; }
        public string? ImagenPath { get; set; } 
        public string? MasInfo { get; set; }
        public int cantPaginas { get; set; }

        public EstadoPublicacion EstadoPublicacion { get; set; } = EstadoPublicacion.Pendiente;

        // Relaciones con otras tablas (opcional)
        public virtual Categorias? Categoria { get; set; }
        public virtual Condiciones? Condicion { get; set; }  // Relación con la tabla Condicione

        public virtual IdiomasEntity? Idioma {  get; set; }
        public int IdiomaId { get; set; } // 👈 FK correcta
        public int FormatoId { get; set; }
        public virtual FormatoEntity? Formato { get; set; }
        public int SubCategoriasId { get; set; }
        public virtual SubCategorias? SubCategorias { get; set;}


        public string PublicadorId { get; set; }
        public Users Publicador { get; set; } 
    }
}
