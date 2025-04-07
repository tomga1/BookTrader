using Microsoft.AspNetCore.Identity;

namespace BookTrader.Models
{
    public class Users : IdentityUser
    {
        public string? NombreCompleto { get; set; }
        public Localidades? Localidad  { get; set; }
    }
}
