using System.ComponentModel.DataAnnotations;

namespace BookShop.Core.Models.Authentication
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 3)]
        public string UserName { get; set; }
        
        [Required]
        [StringLength(16, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
