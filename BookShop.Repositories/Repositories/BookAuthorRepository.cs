using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Abstract.Repositories.Base;
using BookShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Repositories.Repositories
{
    public class BookAuthorRepository : IBookAuthorRepository
    {
        private readonly ACtx ctx;
        private readonly IAsyncRepository<BookAuthor> repo;
        private readonly IAsyncLinqHelper<BookAuthor> helper;

        public BookAuthorRepository(ACtx ctx, IAsyncRepository<BookAuthor> repo, IAsyncLinqHelper<BookAuthor> helper)
        {
            this.ctx = ctx;
            this.repo = repo;
            this.helper = helper;
        }

        public async Task Create(BookAuthor entity)
        {
            await repo.Create(entity);
        }

        public async Task Delete(BookAuthor entity)
        {
            await repo.Delete(entity);
        }

        public async Task<IList<BookAuthor>> GetAll()
        {
            return await repo.GetAll();
        }

        public async Task<BookAuthor> GetById(int id)
        {
            return await repo.GetById(id);
        }

        public bool IsUniqueName(string name)
        {
            return !ctx.BookAuthors.Any(ent => ent.Name == name);
        }

        public async Task Update(BookAuthor entity)
        {
            await repo.Update(entity);
        }

        public async Task<IList<BookAuthor>> Where(Func<BookAuthor, bool> predicate)
        {
            return await helper.Where(predicate);
        }
    }
}
