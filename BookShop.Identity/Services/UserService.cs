﻿using BookShop.Core;
using BookShop.Core.Abstract.Identity;
using BookShop.Core.Abstract.Repositories;
using BookShop.Domain.Entities;
using BookShop.Identity.Configuration;
using BookShop.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookShop.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> user_manager;
        private readonly IBookRepository productRepository;

        public UserService(UserManager<AppUser> user_manager, IBookRepository productRepository, IOptions<JwtSettings> jwt_settings)
        {
            this.user_manager = user_manager;
            this.productRepository = productRepository;
            this.jwt_settings = jwt_settings.Value;
        }

        public async Task<bool> AddOwnedProduct(string user_id, int product_id)
        {
            AppUser user = await user_manager.FindByIdAsync(user_id);

            if (user == null)
                return false;

            Book product = await productRepository.GetById(product_id);

            if (product == null)
                return false;

            user.AddOwnedProduct(product.Id, product.Name, product.FilePath);

            IdentityResult result = await user_manager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> ChangeRole(string user_id, Role role)
        {
            AppUser user = await user_manager.FindByIdAsync(user_id);

            if (user == null)
                return false;

            IList<Claim> claims = await user_manager.GetClaimsAsync(user);
            IdentityResult result = await user_manager.RemoveClaimAsync(user, claims.FirstOrDefault(c => c.Type == "Role"));
            if (!result.Succeeded)
                return false;

            switch (role)
            {
                case Role.DefaultUser:
                    result = await user_manager.AddClaimAsync(user, new Claim("Role", "DefaultUser"));
                    return result.Succeeded;
                case Role.Manager:
                    result = await user_manager.AddClaimAsync(user, new Claim("Role", "Manager"));
                    return result.Succeeded;
                case Role.Administrator:
                    result = await user_manager.AddClaimAsync(user, new Claim("Role", "Administrator"));
                    return result.Succeeded;
                default:
                    return false;
            }
        }

        public async Task<string> GetRole(string user_id)
        {
            AppUser user = await user_manager.FindByIdAsync(user_id);

            if (user == null)
                return string.Empty;

            return (await user_manager.GetClaimsAsync(user)).FirstOrDefault(claim => claim.Type == "Role").Value;
        }
    }
}