using BookShop.Core.Abstract.Repositories.Base;
using BookShop.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookShop.Core.Abstract.Repositories
{
    public interface IBookRepository : IHasBaseRepository<Book>
    {
        Task<bool> ContainsWithName(string name);

        Task<bool> IsUniqueName(string name);

        Task<Book> GetById(int id);

        Task<Book> GetByName(string name);

        Task<IList<Book>> GetAll();

        Task<IList<Book>> GetNewest(int count);

        Task<IList<Book>> GetByPrice(decimal min, decimal max);
                        
        Task<IList<Book>> GetByCategory(Category category);
                        
        Task<IList<Book>> GetByCategoryName(string category_name);
                        
        Task<IList<Book>> GetByAuthor(Author author);
                        
        Task<IList<Book>> GetByAuthorName(string author_name);

        Task<IList<Book>> GetByFilters(IList<Category> categories, IList<Author> authors, decimal PriceMin, decimal PriceMax);
    }
}
