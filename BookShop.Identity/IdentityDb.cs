using BookShop.Core;
using BookShop.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BookShop.Identity
{
    public class IdentityDb : IdentityDbContext<AppUser>
    {
        public IdentityDb(DbContextOptions<IdentityDb> options, IConfiguration configuration) : base(options)
        {
            if (configuration.IsDevelopment())
            {
                Database.EnsureDeleted();
                Database.EnsureCreated();
            }
        }
    }
}
