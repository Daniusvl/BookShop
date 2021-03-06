using System;

namespace BookShop.CRM.Core.Models
{
    public class AuthenticatedUser
    {
        public string UserId { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime DateTokenGot { get; set; } = DateTime.Now;
    }
}
