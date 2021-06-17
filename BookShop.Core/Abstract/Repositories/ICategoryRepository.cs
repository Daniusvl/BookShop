using BookShop.Core.Abstract.Repositories.Base;
using BookShop.Domain.Entities;

namespace BookShop.Core.Abstract.Repositories
{
    public interface ICategoryRepository : IAsyncRepository<Category>, IAsyncLinqHelper<Category>
    {
    }
}
