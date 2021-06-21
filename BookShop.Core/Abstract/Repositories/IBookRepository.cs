using BookShop.Core.Abstract.Repositories.Base;
using BookShop.Domain.Entities;
using System.Collections.Generic;

namespace BookShop.Core.Abstract.Repositories
{
    public interface IBookRepository : IAsyncRepository<Book>, IAsyncLinqHelper<Book>
    {
        bool ContainsWithName(string name);

        bool IsUniqueName(string name);

        IList<Book> GetByName(string name);

        IList<Book> GetByPrice(decimal min, decimal max);

        IList<Book> GetByCategory(Category category);

        IList<Book> GetByCategoryName(string category_name);

        IList<Book> GetByAuthor(Author author);

        IList<Book> GetByAuthorName(string author_name);
    }
}
