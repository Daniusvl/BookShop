using System.ComponentModel.DataAnnotations;

namespace BookShop.Core.Models.Authentication
{
    public class AuthenticationRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
