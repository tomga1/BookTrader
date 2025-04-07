using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BookTrader.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "El Password es requerido")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "The {0} must be at {2} and at max {1} characters long.")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "La contraseña no coincide.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar la contraseña")]
        public string ConfirmPassword { get; set; }




        [Required(ErrorMessage = "Debes seleccionar un país")]
        [Display(Name = "País")]
        public int? PaisId { get; set; }

        [Required(ErrorMessage = "Debes seleccionar una provincia")]
        [Display(Name = "Provincia")]
        public int? ProvinciaId { get; set; }

        [Required(ErrorMessage = "Debes seleccionar una localidad")]
        [Display(Name = "Localidad")]
        public int?  LocalidadId{ get; set; }

        public List<SelectListItem> Provincias { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Paises { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Localidades { get; set; } = new List<SelectListItem>();


    }
}
