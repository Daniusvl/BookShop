using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Abstract.Repositories.Base;
using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Books.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BooksDb ctx;
        public IAsyncRepository<Book> BaseRepository { get; }

        public BookRepository(BooksDb ctx, IAsyncRepository<Book> _base)
        {
            this.ctx = ctx;
            BaseRepository = _base;
        }

        public async Task<bool> ContainsWithName(string name)
        {
            return await ctx.Books.Where(b => !b.Hidden).AnyAsync(ent => ent.Name == name);
        }

        public async Task<IList<Book>> GetByAuthor(Author author)
        {
            return await ctx.Books
                .Where(b => !b.Hidden)
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Photos)
                .Where(ent => ent.Author == author)
                .ToListAsync();
        }

        public async Task<IList<Book>> GetByAuthorName(string author_name)
        {
            return await ctx.Books
                .Where(b => !b.Hidden)
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Photos)
                .Where(ent => ent.Author.Name == author_name)
                .ToListAsync();
        }

        public async Task<IList<Book>> GetByCategory(Category category)
        {
            return await ctx.Books
                .Where(b => !b.Hidden)
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Photos)
                .Where(ent => ent.Category == category).ToListAsync();
        }

        public async Task<IList<Book>> GetByCategoryName(string category_name)
        {
            return await ctx.Books
                .Where(b => !b.Hidden)
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Photos)
                .Where(ent => ent.Category.Name == category_name)
                .ToListAsync();
        }

        public async Task<Book> GetByName(string name)
        {
            return await ctx.Books
                .Where(b => !b.Hidden)
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Photos)
                .FirstOrDefaultAsync(ent => ent.Name == name);
        }

        public async Task<IList<Book>> GetByPrice(decimal min, decimal max)
        {
            return await ctx.Books
                .Where(b => !b.Hidden)
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Photos)
                .Where(b => b.Price >= min && b.Price <= max)
                .ToListAsync();
        }

        public async Task<bool> IsUniqueName(string name)
        {
            return !await ContainsWithName(name);
        }

        public async Task<IList<Book>> GetNewest(int count)
        {
            return ctx.Books
                .Where(b => !b.Hidden)
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Photos)
                .AsEnumerable()
                .TakeLast(count)
                .ToList();
        }

        public async Task<Book> GetById(int id)
        {
            Book book = await BaseRepository.GetById(id);
            if(book != null)
            {
                ctx.Entry(book).Reference(e => e.Author).Load();
                ctx.Entry(book).Reference(e => e.Category).Load();
                ctx.Entry(book).Collection(e => e.Photos).Load();
            }
            return book;
        }

        public async Task<IList<Book>> GetAll()
        {
            return await ctx.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Photos)
                .ToListAsync();
        }

        public async Task<IList<Book>> GetByFilters(IList<Category> categories, IList<Author> authors, decimal PriceMin, decimal PriceMax)
        {
            return await ctx.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Photos)
                .Where(b => categories.Contains(b.Category) &&
                            authors.Contains(b.Author) && 
                            b.Price >= PriceMin && 
                            b.Price <= PriceMax)
                .ToListAsync();
        }
    }
}
