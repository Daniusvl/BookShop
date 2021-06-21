using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Abstract.Repositories.Base;
using BookShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Repositories.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly BooksDb ctx;
        private readonly IAsyncRepository<Product> repo;
        private readonly IAsyncLinqHelper<Product> helper;

        public ProductRepository(BooksDb ctx, IAsyncRepository<Product> repo, IAsyncLinqHelper<Product> helper)
        {
            this.ctx = ctx;
            this.repo = repo;
            this.helper = helper;
        }

        public bool ContainsWithName(string name)
        {
            return ctx.Products.Any(ent => ent.Name == name);
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

        public IList<Product> GetByAuthor(BookAuthor author)
        {
            return ctx.Products.Where(ent => ent.Author == author).ToList();
        }

        public IList<Product> GetByAuthorName(string author_name)
        {
            return ctx.Products.Where(ent => ent.Author.Name == author_name).ToList();
        }

        public IList<Product> GetByCategory(Category category)
        {
            return ctx.Products.Where(ent => ent.Category == category).ToList();
        }

        public IList<Product> GetByCategoryName(string category_name)
        {
            return ctx.Products.Where(ent => ent.Category.Name == category_name).ToList();
        }

        public async Task<Product> GetById(int id)
        {
            return await repo.GetById(id);
        }

        public IList<Product> GetByName(string name)
        {
            return ctx.Products.Where(ent => ent.Name == name).ToList();
        }

        public IList<Product> GetByPrice(decimal min, decimal max)
        {
            throw new NotImplementedException();
        }

        public bool IsUniqueName(string name)
        {
            return !ContainsWithName(name);
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
