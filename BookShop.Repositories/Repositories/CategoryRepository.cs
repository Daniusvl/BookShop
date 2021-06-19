using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Abstract.Repositories.Base;
using BookShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookShop.Repositories.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ACtx ctx;
        private readonly IAsyncRepository<Category> repo;
        private readonly IAsyncLinqHelper<Category> helper;

        public CategoryRepository(ACtx ctx, IAsyncRepository<Category> repo, IAsyncLinqHelper<Category> helper)
        {
            this.ctx = ctx;
            this.repo = repo;
            this.helper = helper;
        }

        public async Task Create(Category entity)
        {
            await repo.Create(entity);
        }

        public async Task Delete(Category entity)
        {
            await repo.Delete(entity);
        }

        public async Task<IList<Category>> GetAll()
        {
            return await repo.GetAll();
        }

        public async Task<Category> GetById(int id)
        {
            return await repo.GetById(id);
        }

        public async Task Update(Category entity)
        {
            await repo.Update(entity);
        }

        public async Task<IList<Category>> Where(Func<Category, bool> predicate)
        {
            return await helper.Where(predicate);
        }
    }
}
