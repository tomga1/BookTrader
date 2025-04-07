namespace BookTrader.Models
{
    public class Paises
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public ICollection<Provincias> Provincia { get; set; }

    }
}
