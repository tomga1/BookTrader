using System.ComponentModel.DataAnnotations;

namespace BookTrader.Models
{

    public enum TipoSugerencia
    {
        Idea,
        Sugerencia,
        Error
    }
    public class Sugerencias
    {
        public int Id { get; set; }
        [Required]
        public TipoSugerencia Tipo { get; set; }

        [Required]
        public string? Descripcion { get; set; } = string.Empty;

        public DateTime Fecha { get; set; } = DateTime.Now; 


    }
}
