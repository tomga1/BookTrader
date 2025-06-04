using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookTrader.Models
{
    public class PlanesSuscripcion
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
        public int CantidadLibrosPorDia { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }

    }
}
