using System.ComponentModel.DataAnnotations;

namespace BookTrader.ViewModels
{
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [EmailAddress]
        public string Email { get; set; }


    }
}
