using System.Threading.Tasks;

namespace BookShop.Core.Abstract.Identity
{
    public interface IUserService
    {
        Task<bool> AddOwnedProduct(string user_id, int product_id);

        Task<bool> ChangeRole(string user_id, Role role);

        Task<string> GetRole(string user_id);
    }
}
