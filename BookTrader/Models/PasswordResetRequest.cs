using System.ComponentModel.DataAnnotations;

namespace BookTrader.Models
{
    public class PasswordResetRequest
    {
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public Guid GuidCode { get; set; }
        public DateTime Expiration { get; set; }
        public bool Used { get; set; }

    }
}
