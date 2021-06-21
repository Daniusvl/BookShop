using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BookShop.Identity.Models
{
    public class AppUser : IdentityUser
    {
        public AppUser() { }

        public AppUser(string userName) : base (userName){}

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
    }
}
