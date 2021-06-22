using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Abstract.Repositories.Base;
using BookShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Books.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BooksDb ctx;
        private readonly IAsyncRepository<Book> repo;
        private readonly IAsyncLinqHelper<Book> helper;

        public BookRepository(BooksDb ctx, IAsyncRepository<Book> repo, IAsyncLinqHelper<Book> helper)
        {
            this.ctx = ctx;
            this.repo = repo;
            this.helper = helper;
        }

        public bool ContainsWithName(string name)
        {
            return ctx.Books.Any(ent => ent.Name == name);
        }

        public async Task Create(Book entity)
        {
            await repo.Create(entity);
        }

        public async Task Delete(Book entity)
        {
            await repo.Delete(entity);
        }

        public async Task<IList<Book>> GetAll()
        {
            return await repo.GetAll();
        }

        public IList<Book> GetByAuthor(Author author)
        {
            return ctx.Books.Where(ent => ent.Author == author).ToList();
        }

        public IList<Book> GetByAuthorName(string author_name)
        {
            return ctx.Books.Where(ent => ent.Author.Name == author_name).ToList();
        }

        public IList<Book> GetByCategory(Category category)
        {
            return ctx.Books.Where(ent => ent.Category == category).ToList();
        }

        public IList<Book> GetByCategoryName(string category_name)
        {
            return ctx.Books.Where(ent => ent.Category.Name == category_name).ToList();
        }

        public async Task<Book> GetById(int id)
        {
            return await repo.GetById(id);
        }

        public Book GetByName(string name)
        {
            return ctx.Books.FirstOrDefault(ent => ent.Name == name);
        }

        public IList<Book> GetByPrice(decimal min, decimal max)
        {
            throw new NotImplementedException();
        }

        public bool IsUniqueName(string name)
        {
            return !ContainsWithName(name);
        }

        public async Task Update(Book entity)
        {
            await repo.Update(entity);
        }

        public async Task<IList<Book>> Where(Func<Book, bool> predicate)
        {
            return await helper.Where(predicate);
        }
    }
}
