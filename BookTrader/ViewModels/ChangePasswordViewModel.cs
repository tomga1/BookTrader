using System.ComponentModel.DataAnnotations;

namespace BookTrader.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "El Password es requerido")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "The {0} must be at {2} and at max {1} characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "nueva contraseña")]
        [Compare("ConfirmNewPassword", ErrorMessage = "La contraseña no coincide.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar la nueva contraseña")]
        public string ConfirmNewPassword { get; set; }
    }
}
