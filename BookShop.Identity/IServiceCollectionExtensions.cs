using BookShop.Core.Abstract.Identity;
using BookShop.Identity.Authorization;
using BookShop.Identity.Configuration;
using BookShop.Identity.Models;
using BookShop.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Text;

namespace BookShop.Identity
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.AddDbContext<IdentityDb>(options =>
                options.UseSqlServer(configuration.GetConnectionString("IdentityDb")));

            services.AddIdentityCore<AppUser>(options => 
            {
                options.Password.RequireNonAlphanumeric = false;
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
            })
                .AddEntityFrameworkStores<IdentityDb>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, configure =>
                {
                    configure.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        ValidAudience = configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddAuthorization(cfg =>
            {
                cfg.AddPolicy(RoleConstants.DefaultUserName, policy_cfg =>
                    policy_cfg.RequireClaim(ClaimTypes.Role, RoleConstants.DefaultUser));

                cfg.AddPolicy(RoleConstants.ModeratorName, policy_cfg =>
                    policy_cfg.RequireClaim(ClaimTypes.Role, RoleConstants.Moderator));

                cfg.AddPolicy(RoleConstants.AdministratorName, policy_cfg =>
                    policy_cfg.RequireClaim(ClaimTypes.Role, RoleConstants.Administrator));

            });

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
