using BookShop.Domain.Entities.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookShop.Core.Abstract.Repositories.Base
{
    public interface IAsyncRepository<TEntity> 
        where TEntity : BaseEntity
    {
        Task<IList<TEntity>> GetAll();

        Task<TEntity> GetById(int id);

        Task Create(TEntity entity);

        Task Update(TEntity entity);

        Task Delete(TEntity entity);
    }
}
