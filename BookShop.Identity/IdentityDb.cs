using BookShop.Features.Configuration;
using BookShop.Identity.Authorization;
using BookShop.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;

namespace BookShop.Identity
{
    public class IdentityDb : IdentityDbContext<AppUser>
    {
        private readonly Email email;

        public IdentityDb(DbContextOptions<IdentityDb> options, IOptions<Email> email) : base(options)
        {
            this.email = email.Value;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            string user_id = "1";
            int claim_id = 999999999;

            PasswordHasher<AppUser> hasher = new();

            AppUser user = new()
            {
                Id = user_id,
                Email = email.UserName,
                NormalizedEmail = email.UserName.ToUpper(),
                EmailConfirmed = true,
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                SecurityStamp = string.Empty,
                RefreshToken = "FIRST_REFRESH_TOKEN",
                RefreshTokenExpires = DateTime.UtcNow.AddDays(5),
                PasswordHash = hasher.HashPassword(null, email.Password)
            };

            builder.Entity<AppUser>()
                .HasData(user);

            IdentityUserClaim<string> claim = new()
            {
                Id = claim_id,
                UserId = user_id,
                ClaimType = RoleConstants.RoleClaim,
                ClaimValue = RoleConstants.AdministratorName
            };

            builder.Entity<IdentityUserClaim<string>>()
                .HasData(claim);
        }
    }
}
