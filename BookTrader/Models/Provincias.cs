namespace BookTrader.Models
{
    public class Provincias
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public int PaisId { get; set; }
        public Paises Pais { get; set; }

        public ICollection<Localidades> Localidades { get; set; }
    }
}
