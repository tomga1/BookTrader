using System.ComponentModel.DataAnnotations;

namespace BookTrader.ViewModels
{
    public class MiProfileViewModel
    {
        
        [Phone]
        [Display(Name = "Teléfono")]
        public string PhoneNumber { get; set; }
    }
}
