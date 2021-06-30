using BookShop.Core;
using BookShop.Core.Abstract.Identity;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Domain.Entities;
using BookShop.Identity.Models;
using Microsoft.AspNetCore.Identity;
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

        public UserService(UserManager<AppUser> user_manager, IBookRepository productRepository)
        {
            this.user_manager = user_manager;
            this.productRepository = productRepository;
        }

        public async Task AddOwnedProduct(string user_id, int book_id)
        {
            AppUser user = await user_manager.FindByIdAsync(user_id);

            if (user == null)
                throw new NotFoundException(nameof(AppUser), user_id);

            Book product = await productRepository.BaseRepository.GetById(book_id);

            if (product == null)
                throw new NotFoundException(nameof(Book), book_id);

            user.AddOwnedProduct(product.Id, product.Name, product.FilePath);

            IdentityResult result = await user_manager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new CommonException(string.Join(null, result.Errors.Select(error => error.Description)));
        }

        public async Task ChangeRole(string user_id, Role role)
        {
            AppUser user = await user_manager.FindByIdAsync(user_id);

            if (user == null)
                throw new NotFoundException(nameof(AppUser), user_id);

            IList<Claim> claims = await user_manager.GetClaimsAsync(user);
            IdentityResult result = await user_manager.RemoveClaimAsync(user, claims.FirstOrDefault(c => c.Type == "Role"));
            if (!result.Succeeded)
                throw new CommonException(string.Join(null, result.Errors.Select(error => error.Description)));

            switch (role)
            {
                case Role.DefaultUser:
                    result = await user_manager.AddClaimAsync(user, new Claim("Role", "DefaultUser"));
                    break;
                case Role.Manager:
                    result = await user_manager.AddClaimAsync(user, new Claim("Role", "Manager"));
                    break;
                case Role.Administrator:
                    result = await user_manager.AddClaimAsync(user, new Claim("Role", "Administrator"));
                    break;
                default:
                    throw new CommonException("Role name error");
            }
        }

        public async Task<string> GetRole(string user_id)
        {
            AppUser user = await user_manager.FindByIdAsync(user_id);

            if (user == null)
                throw new NotFoundException(nameof(AppUser), user_id);

            string claim_value = (await user_manager.GetClaimsAsync(user))?
                .FirstOrDefault(claim => claim.Type == "Role")?.Value;

            if (claim_value == null)
                throw new NotFoundException(nameof(Claim), "Role");

            return claim_value;
        }
    }
}
