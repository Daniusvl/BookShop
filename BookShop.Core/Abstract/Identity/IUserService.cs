using System.Threading.Tasks;

namespace BookShop.Core.Abstract.Identity
{
    public interface IUserService
    {
        Task AddOwnedProduct(string user_id, int product_id);

        Task ChangeRole(string user_id, Role role);

        Task<string> GetRole(string user_id);
    }
}
