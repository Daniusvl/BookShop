using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Abstract.Repositories.Base;
using BookShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Repositories.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BooksDb ctx;
        private readonly IAsyncRepository<Author> repo;
        private readonly IAsyncLinqHelper<Author> helper;

        public AuthorRepository(BooksDb ctx, IAsyncRepository<Author> repo, IAsyncLinqHelper<Author> helper)
        {
            this.ctx = ctx;
            this.repo = repo;
            this.helper = helper;
        }

        public async Task Create(Author entity)
        {
            await repo.Create(entity);
        }

        public async Task Delete(Author entity)
        {
            await repo.Delete(entity);
        }

        public async Task<IList<Author>> GetAll()
        {
            return await repo.GetAll();
        }

        public async Task<Author> GetById(int id)
        {
            return await repo.GetById(id);
        }

        public bool IsUniqueName(string name)
        {
            return !ctx.Authors.Any(ent => ent.Name == name);
        }

        public async Task Update(Author entity)
        {
            await repo.Update(entity);
        }

        public async Task<IList<Author>> Where(Func<Author, bool> predicate)
        {
            return await helper.Where(predicate);
        }
    }
}
