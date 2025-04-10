namespace BookTrader.Models
{
    public class SubCategorias : EntityBase
    {
        public int CategoriaId { get; set; }
        public Categorias Categoria { get; set; } 
        public string Nombre { get; set; } 

    }
}
