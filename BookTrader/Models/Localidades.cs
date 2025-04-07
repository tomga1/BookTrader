namespace BookTrader.Models
{
    public class Localidades
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public Provincias Provincia { get; set; }
    }
}
