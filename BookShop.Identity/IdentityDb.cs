using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Identity
{
    public class IdentityDb : IdentityDbContext<IdentityUser>
    {
        public IdentityDb(DbContextOptions<IdentityDb> options) : base(options)
        {
        }
    }
}
