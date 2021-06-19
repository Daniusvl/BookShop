using BookShop.Core.Abstract.Repositories.Base;
using BookShop.Domain.Entities;

namespace BookShop.Core.Abstract.Repositories
{
    public interface IBookAuthorRepository : IAsyncRepository<BookAuthor>, IAsyncLinqHelper<BookAuthor>
    {
        bool IsUniqueName(string name);
    }
}
