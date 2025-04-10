namespace BookTrader.Models
{
    public class Categorias : EntityBase
    {
       public virtual ICollection<Libros>? Libros { get; set; }
        public ICollection<SubCategorias> SubCategorias { get; set; }

    }
}
