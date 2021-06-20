using BookShop.Core.Configuration;
using BookShop.Identity.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Identity
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthenticationAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityDb>(options =>
            {
                if (configuration.IsDevelopment())
                {
                    options.UseInMemoryDatabase("IdentityDb");
                }
                else
                {
                    options.UseSqlServer(configuration.GetConnectionString("IdentityDb"));
                }
            });

            services.AddIdentityCore<IdentityUser>(options => 
            {
                options.Password.RequireNonAlphanumeric = false;
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
            })
                .AddEntityFrameworkStores<IdentityDb>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

            services.AddAuthorization(cfg =>
            {
                cfg.AddPolicy(RoleConstants.DefaultUserName, policy_cfg =>
                    policy_cfg.RequireClaim(RoleConstants.RoleClaim, RoleConstants.DefaultUser));

                cfg.AddPolicy(RoleConstants.ModeratorName, policy_cfg =>
                    policy_cfg.RequireClaim(RoleConstants.RoleClaim, RoleConstants.Moderator));

                cfg.AddPolicy(RoleConstants.AdministratorName, policy_cfg =>
                    policy_cfg.RequireClaim(RoleConstants.RoleClaim, RoleConstants.Administrator));

            });

            return services;
        }
    }
}
