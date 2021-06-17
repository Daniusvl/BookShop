using BookShop.Domain.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookShop.Core.Abstract.Repositories.Base
{
    public interface IAsyncLinqHelper<TEntity>
        where TEntity : BaseEntity
    {
        Task<IList<TEntity>> Where(Func<TEntity, bool> predicate);
    }
}
