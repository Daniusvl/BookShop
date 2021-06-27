using BookShop.Domain.Entities.Abstract;

namespace BookShop.Core.Abstract.Repositories.Base
{
    public interface IHasBaseRepository<TEntity>
        where TEntity : BaseEntity
    {
        IAsyncRepository<TEntity> BaseRepository {get;}
    }
}
