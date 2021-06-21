using BookShop.Core.Abstract.Repositories.Base;
using BookShop.Domain.Entities.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Repositories.Repositories
{
    public class AsyncRepository<TEntity> : IAsyncRepository<TEntity>
        where TEntity : BaseEntity
    {
        private readonly BooksDb ctx;

        public AsyncRepository(BooksDb ctx)
        {
            this.ctx = ctx;
        }

        public async Task Create(TEntity entity)
        {
            await ctx.Set<TEntity>()
                .AddAsync(entity);
            await ctx.SaveChangesAsync();
        }

        public async Task Delete(TEntity entity)
        {
            ctx.Set<TEntity>()
                .Remove(entity);
            await ctx.SaveChangesAsync();
        }

        public async Task<IList<TEntity>> GetAll()
        {
            return ctx.Set<TEntity>()
                .ToList();
        }

        public async Task<TEntity> GetById(int id)
        {
            return ctx.Set<TEntity>()
                .FirstOrDefault(ent => ent.Id == id);
        }

        public async Task Update(TEntity entity)
        {
            ctx.Set<TEntity>()
                .Update(entity);
            await ctx.SaveChangesAsync();
        }
    }
}
