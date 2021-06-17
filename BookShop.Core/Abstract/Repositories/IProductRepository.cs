using BookShop.Core.Abstract.Repositories.Base;
using BookShop.Domain.Entities;

namespace BookShop.Core.Abstract.Repositories
{
    public interface IProductRepository : IAsyncRepository<Product>, IAsyncLinqHelper<Product>
    {
    }
}
