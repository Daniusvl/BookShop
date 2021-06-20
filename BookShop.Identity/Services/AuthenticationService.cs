using BookShop.Core.Abstract.Identity;
using BookShop.Core.Models.Authentication;
using System;
using System.Threading.Tasks;

namespace BookShop.Identity.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public Task<AuthenticationModel> Authenticate(AuthenticationRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<RegisterModel> Register(RegisterRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
