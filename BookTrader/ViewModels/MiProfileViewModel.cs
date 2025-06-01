using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BookTrader.ViewModels
{
    public class MiProfileViewModel
    {
        
        [Phone]
        [Display(Name = "Teléfono")]
        public string PhoneNumber { get; set; }



        [Required(ErrorMessage = "Debes seleccionar un país")]
        [Display(Name = "País")]
        public int? PaisId { get; set; }

        [Required(ErrorMessage = "Debes seleccionar una provincia")]
        [Display(Name = "Provincia")]
        public int? ProvinciaId { get; set; }

        [Required(ErrorMessage = "Debes seleccionar una localidad")]
        [Display(Name = "Localidad")]
        public int? LocalidadId { get; set; }

        public List<SelectListItem> Provincias { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Paises { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Localidades { get; set; } = new List<SelectListItem>();
    }
}
