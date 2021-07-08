using BookShop.Core.Abstract.Repositories.Base;
using BookShop.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookShop.Core.Abstract.Repositories
{
    public interface ICategoryRepository : IHasBaseRepository<Category>
    {
        Task<bool> IsUniqueName(string name);

        Task<IList<Category>> SearchByName(string str);
    }
}
