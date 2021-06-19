using BookShop.Core.Abstract.Repositories.Base;
using BookShop.Domain.Entities;
using System.Collections.Generic;

namespace BookShop.Core.Abstract.Repositories
{
    public interface IProductRepository : IAsyncRepository<Product>, IAsyncLinqHelper<Product>
    {
        bool ContainsWithName(string name);

        bool IsUniqueName(string name);

        IList<Product> GetByPrice(decimal min, decimal max);

        IList<Product> GetByCategory(Category category);

        IList<Product> GetByCategoryName(string category_name);

        IList<Product> GetByAuthor(BookAuthor author);

        IList<Product> GetByAuthorName(string author_name);
    }
}
