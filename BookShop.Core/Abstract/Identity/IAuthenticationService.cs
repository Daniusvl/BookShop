using BookShop.Core.Models.Authentication;
using System.Threading.Tasks;

namespace BookShop.Core.Abstract.Identity
{
    public interface IAuthenticationService
    {
        Task<RegisterModel> Register(RegisterRequest request);

        Task<AuthenticationModel> Authenticate(AuthenticationRequest request);

        Task<bool> AddOwnedProduct(string user_id, int product_id);
    }
}
