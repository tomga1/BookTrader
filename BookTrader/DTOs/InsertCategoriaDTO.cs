using System.Security.Principal;

namespace BookTrader.DTOs
{
    public class InsertCategoriaDTO
    {
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaPublicacion { get; set; } = DateTime.Now;

        public DateTime FechaAgregado { get; set; } = DateTime.Now;
        public bool Estado { get; set; } = true; 


    }
}
