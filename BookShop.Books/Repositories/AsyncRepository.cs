using BookShop.Core.Abstract.Repositories.Base;
using BookShop.Domain.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookShop.Books.Repositories
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
            return await ctx.Set<TEntity>()
                .ToListAsync();
        }

        public async Task<TEntity> GetById(int id)
        {
            return await ctx.Set<TEntity>()
                .FirstOrDefaultAsync(ent => ent.Id == id);
        }

        public async Task Update(TEntity entity)
        {
            ctx.Set<TEntity>()
                .Update(entity);
            await ctx.SaveChangesAsync();
        }
    }
}
