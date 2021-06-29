using BookShop.CRM.Core.Models;
using System.Threading.Tasks;

namespace BookShop.CRM.Core.Base
{
    public interface IAuthenticationService
    {
        Task<AuthenticatedUser> Authenticate(AuthenticateCommand command);
    }
}
