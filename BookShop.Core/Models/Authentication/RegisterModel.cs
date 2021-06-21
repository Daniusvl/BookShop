using System.Collections.Generic;

namespace BookShop.Core.Models.Authentication
{
    public class RegisterModel
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public bool Success { get; set; }

        public IList<string> PasswordErrors { get; set; }
    }
}
