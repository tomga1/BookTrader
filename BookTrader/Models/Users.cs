using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookTrader.Models
{
    public class Users : IdentityUser
    {
        public string? NombreCompleto { get; set; }
        public Localidades? Localidad  { get; set; }
        public bool IsFirstLogin { get; set; } = true;

        public int? IdPlanSuscripcion { get; set; }

        [ForeignKey("IdPlanSuscripcion")]
        public PlanesSuscripcion PlanesSuscripcion { get; set; }

    }
}
