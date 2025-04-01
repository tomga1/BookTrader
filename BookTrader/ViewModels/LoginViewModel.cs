using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BookTrader.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Se requiere un Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Se requiere una contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
