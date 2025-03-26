using BookTrader.Models;
using System.ComponentModel.DataAnnotations;

namespace BookTrader.DTOs
{
    public class InsertSugerenciaDTO
    {
        [Required]
        public TipoSugerencia Tipo { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
        public string? Descripcion { get; set; } = string.Empty;

        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}
