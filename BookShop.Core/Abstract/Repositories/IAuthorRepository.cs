using BookShop.Core.Abstract.Repositories.Base;
using BookShop.Domain.Entities;

namespace BookShop.Core.Abstract.Repositories
{
    public interface IAuthorRepository : IAsyncRepository<Author>, IAsyncLinqHelper<Author>
    {
        bool IsUniqueName(string name);
    }
}
