using System.ComponentModel.DataAnnotations;

namespace BookShop.Core.Models.Authentication
{
    public class RefreshTokenRequest
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}
