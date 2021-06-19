using BookShop.Core.Abstract.Repositories.Base;
using BookShop.Domain.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Repositories.Repositories
{
    public class AsyncLinqHelper<TEntity> : IAsyncLinqHelper<TEntity>
        where TEntity : BaseEntity
    {
        private readonly ACtx ctx;

        public AsyncLinqHelper(ACtx ctx)
        {
            this.ctx = ctx;
        }

        public async Task<IList<TEntity>> Where(Func<TEntity, bool> predicate)
        {
            return ctx.Set<TEntity>()
                .Where(predicate)
                .ToList();
        }
    }
}
