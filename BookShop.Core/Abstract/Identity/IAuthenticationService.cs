using BookShop.Core.Models.Authentication;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BookShop.Core.Abstract.Identity
{
    public interface IAuthenticationService
    {
        Task<RegisterModel> Register(RegisterRequest request, HttpRequest http_request);

        Task<AuthenticationModel> Authenticate(AuthenticationRequest request);

        Task ConfirmEmail(string user_id, string email_token);

        Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request);
    }
}
