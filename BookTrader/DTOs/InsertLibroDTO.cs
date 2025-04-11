using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace BookTrader.DTOs
{
    public class InsertLibroDTO
    {
        [Required]
        public string Nombre { get; set; } = string.Empty;
        [Required]
        public string Autor { get; set; } = string.Empty;
        [Required]
        public string Editorial { get; set; } = string.Empty;
        public string? ISBN { get; set; }
        public int CategoriaId { get; set; }
        public int CondicionId { get; set; }
        public int IdiomaId { get; set; }
        [Required]
        public decimal Precio { get; set; }
        public string? ImagenUrl { get; set; }

        public IFormFile? ImagenArchivo { get; set; }

        public string? MasInfo { get; set; }
        public DateTime FechaPublicacion { get; set; } = DateTime.Now;
        public string? Descripcion { get; set; }
        public int? IdUsuario { get; set; }

        public int cantPaginas { get; set; }
        public int FormatoId { get; set; }
      

    }
}
