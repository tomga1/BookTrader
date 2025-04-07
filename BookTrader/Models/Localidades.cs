namespace BookTrader.Models
{
    public class Localidades
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int ProvinciaId { get; set; }
        public Provincias Provincia { get; set; }
    }
}
