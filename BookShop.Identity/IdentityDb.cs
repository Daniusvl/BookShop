using BookShop.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Identity
{
    public class IdentityDb : IdentityDbContext<AppUser>
    {
        public IdentityDb(DbContextOptions<IdentityDb> options) : base(options)
        {
        }
    }
}
