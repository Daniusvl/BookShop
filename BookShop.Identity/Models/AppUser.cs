using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BookShop.Identity.Models
{
    public class AppUser : IdentityUser
    {
        public AppUser() { }

        public AppUser(string userName) : base (userName){}

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpires { get; set; }

        public string OwnedProducts { get; set; } = "[]";

        public IList<OwnedProduct> GetOwnedProducts()
        {
            return JsonConvert.DeserializeObject<List<OwnedProduct>>(OwnedProducts) ?? new List<OwnedProduct>();
        }

        public void AddOwnedProduct(int id, string name, string file_path)
        {
            IList<OwnedProduct> products = GetOwnedProducts();
            products.Add(new OwnedProduct { Id = id, Name = name, FilePath = file_path });
            OwnedProducts = JsonConvert.SerializeObject(products);
        }

        public string GenerateAndWriteRefreshToken()
        {
            RefreshToken = string.Join(null, Guid.NewGuid().ToString().Split('-'));
            RefreshTokenExpires = DateTime.UtcNow.AddDays(5);
            return RefreshToken;
        }
    }
}
