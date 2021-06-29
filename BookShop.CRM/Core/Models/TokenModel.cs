using System;

namespace BookShop.CRM.Core.Models
{
    public class TokenModel
    {
        public string Token { get; set; } = string.Empty;

        public DateTime DateTokenGot { get; set; } = DateTime.Now;
    }
}
