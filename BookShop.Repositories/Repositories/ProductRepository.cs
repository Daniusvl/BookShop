using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Abstract.Repositories.Base;
using BookShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookShop.Repositories.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ACtx ctx;
        private readonly IAsyncRepository<Product> repo;
        private readonly IAsyncLinqHelper<Product> helper;

        public ProductRepository(ACtx ctx, IAsyncRepository<Product> repo, IAsyncLinqHelper<Product> helper)
        {
            this.ctx = ctx;
            this.repo = repo;
            this.helper = helper;
        }

        public async Task Create(Product entity)
        {
            await repo.Create(entity);
        }

        public async Task Delete(Product entity)
        {
            await repo.Delete(entity);
        }

        public async Task<IList<Product>> GetAll()
        {
            return await repo.GetAll();
        }

        public async Task<Product> GetById(int id)
        {
            return await repo.GetById(id);
        }

        public async Task Update(Product entity)
        {
            await repo.Update(entity);
        }

        public async Task<IList<Product>> Where(Func<Product, bool> predicate)
        {
            return await helper.Where(predicate);
        }
    }
}
