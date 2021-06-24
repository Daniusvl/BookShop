using BookShop.Core;
using BookShop.Core.Abstract.Identity;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Models.Authentication;
using BookShop.Domain.Entities;
using BookShop.Identity.Configuration;
using BookShop.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Identity.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<AppUser> user_manager;
        private readonly IBookRepository productRepository;
        private readonly JwtSettings jwt_settings;

        public AuthenticationService(UserManager<AppUser> user_manager, IBookRepository productRepository, IOptions<JwtSettings> jwt_settings)
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

        public async Task<AuthenticationModel> Authenticate(AuthenticationRequest request)
        {
            AuthenticationModel authentication_model = new();

            AppUser user = await user_manager.FindByEmailAsync(request?.Email);

            if(user == null)
            {
                authentication_model.Success = false;
                return authentication_model;
            }

            bool correct_password = await user_manager.CheckPasswordAsync(user, request?.Passwrod);

            if (!correct_password)
            {
                authentication_model.Success = false;
                return authentication_model;
            }

            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            SigningCredentials credentials = new (
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt_settings.Key)),
                    SecurityAlgorithms.HmacSha256
                );

            JwtSecurityToken token = new JwtSecurityToken(
                    jwt_settings.Issuer,
                    jwt_settings.Audience,
                    claims,
                    DateTime.Now,
                    DateTime.Now.AddHours(1),
                    credentials
                );

            string token_string = new JwtSecurityTokenHandler().WriteToken(token);

            authentication_model.UserId = user.Id;
            authentication_model.UserName = user.UserName;
            authentication_model.Email = user.Email;
            authentication_model.Role = (await user_manager.GetClaimsAsync(user)).FirstOrDefault(claim => claim.Type == "Role").Value;
            authentication_model.Token = token_string;
            authentication_model.Success = true;
            return authentication_model;
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

        public async Task<RegisterModel> Register(RegisterRequest request)
        {
            AppUser user = new AppUser
            {
                UserName = request.UserName,
                Email = request.Email
            };

            IList<string> password_errors = new List<string>();
            foreach (IPasswordValidator<AppUser> password_validator in user_manager.PasswordValidators)
            {
                IdentityResult validation_result = await password_validator.ValidateAsync(user_manager, user, request?.Password);
                if (!validation_result.Succeeded)
                {
                    foreach (IdentityError error in validation_result.Errors)
                    {
                        password_errors.Add(error.Description);
                    }
                }
            }

            if(password_errors.Count > 0)
            {
                return new RegisterModel
                {
                    Success = false,
                    PasswordErrors = password_errors
                };
            }

            IdentityResult result = await user_manager.CreateAsync(user, request?.Password);

            if (!result.Succeeded)
            {
                return new RegisterModel { Success = false };
            }

            RegisterModel registerModel = new RegisterModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Success = true
            };

            result = await user_manager.AddClaimAsync(user, new Claim("Role", "DefaultUser"));

            if (!result.Succeeded)
            {
                return new RegisterModel { Success = false };
            }

            return registerModel;
        }
    }
}
