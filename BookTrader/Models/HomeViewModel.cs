namespace BookTrader.Models
{
    public class HomeViewModel
    {
        public PaginacionViewModel<Libros> LibrosPaginados { get; set; }

        // Propiedad para la lista de categorías
        public IEnumerable<Categorias> Categorias { get; set; }
    }
}
