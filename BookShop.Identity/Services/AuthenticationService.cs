using BookShop.Core.Abstract.Features.EmailSender;
using BookShop.Core.Abstract.Identity;
using BookShop.Core.Exceptions;
using BookShop.Core.Models.Authentication;
using BookShop.Identity.Configuration;
using BookShop.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
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
        private readonly IEmailSender email_sender;
        private readonly JwtSettings jwt_settings;

        public AuthenticationService(UserManager<AppUser> user_manager, IOptions<JwtSettings> jwt_settings, IEmailSender email_sender)
        {
            this.user_manager = user_manager;
            this.email_sender = email_sender;
            this.jwt_settings = jwt_settings.Value;
        }

        public async Task<AuthenticationModel> Authenticate(AuthenticationRequest request)
        {
            AuthenticationModel authentication_model = new();

            AppUser user = await user_manager.FindByEmailAsync(request?.Email);

            if(user == null)
            {
                throw new CommonException("Email or password is incorrect");
            }

            bool correct_password = await user_manager.CheckPasswordAsync(user, request?.Password);
            if (!correct_password)
            {
                throw new CommonException("Email or password is incorrect");
            }

            if (!user.EmailConfirmed)
            {
                throw new CommonException("Email not confirmed");
            }

            authentication_model.UserId = user.Id;
            authentication_model.UserName = user.UserName;
            authentication_model.Email = user.Email;
            authentication_model.Role = (await user_manager.GetClaimsAsync(user)).FirstOrDefault(claim => claim.Type == "Role").Value;
            authentication_model.AccessToken = GenerateToken(user);
            authentication_model.RefreshToken = user.GenerateAndWriteRefreshToken();
            IdentityResult result = await user_manager.UpdateAsync(user);

            return authentication_model;
        }

        public async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request)
        {
            RefreshTokenResponse response = new();
            response.Auth = new();

            AppUser user = await user_manager.FindByIdAsync(request.UserId);

            if(user == null)
            {
                throw new NotFoundException(nameof(AppUser), request.UserId);
            }

            if (!user.EmailConfirmed)
            {
                throw new CommonException("Email not confirmed");
            }

            if(request.RefreshToken != user.RefreshToken)
            {
                throw new CommonException("Invalid refresh token");
            }

            if(user.RefreshTokenExpires < DateTime.UtcNow)
            {
                throw new CommonException("Refresh token expired");
            }

            response.Auth.UserId = user.Id;
            response.Auth.UserName = user.UserName;
            response.Auth.Email = user.Email;
            response.Auth.Role = (await user_manager.GetClaimsAsync(user)).FirstOrDefault(claim => claim.Type == "Role").Value;
            response.Auth.AccessToken = GenerateToken(user);
            response.Auth.RefreshToken = user.GenerateAndWriteRefreshToken();
            IdentityResult result = await user_manager.UpdateAsync(user);

            return response;
        }

        private string GenerateToken(AppUser user)
        {
            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            SigningCredentials credentials = new(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt_settings.Key)),
                    SecurityAlgorithms.HmacSha256
                );

            JwtSecurityToken token = new JwtSecurityToken(
                    jwt_settings.Issuer,
                    jwt_settings.Audience,
                    claims,
                    DateTime.Now,
                    DateTime.Now.AddHours(24),
                    credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<RegisterModel> Register(RegisterRequest request, HttpRequest http_request)
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
                throw new ValidationException(password_errors.ToArray());
            }

            IdentityResult result = await user_manager.CreateAsync(user, request?.Password);

            if (!result.Succeeded)
            {
                throw new CommonException(JsonConvert.SerializeObject(result.Errors.Select(e => e.Description)));
            }

            RegisterModel registerModel = new RegisterModel
            {
                UserId = user.Id,
                UserName = user.UserName
            };

            result = await user_manager.AddClaimAsync(user, new Claim("Role", "DefaultUser"));

            if (!result.Succeeded)
            {
                throw new CommonException(JsonConvert.SerializeObject(result.Errors.Select(e => e.Description)));
            }

            string email_token = await user_manager.GenerateEmailConfirmationTokenAsync(user);

            string url = $"{http_request.Scheme}://{http_request.Host}/api/Auth/EmailConfirmation?user_id={user.Id}&email_token={email_token}";

            await email_sender.SendAsync(user.Email, "Email confirmation", $"<h2>Please confirm email address</h2> <a href=\"{url}\">Link</a>");

            return registerModel;
        }

        public async Task ConfirmEmail(string user_id, string email_token)
        {
            AppUser user = await user_manager.FindByIdAsync(user_id);

            if(user == null)
            {
                throw new NotFoundException(nameof(AppUser), user.Id);
            }

            IdentityResult result = await user_manager.ConfirmEmailAsync(user, email_token);

            if (!result.Succeeded)
            {
                throw new CommonException(JsonConvert.SerializeObject(result.Errors.Select(e => e.Description)));
            }
        }
    }
}
