using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Abstract.Repositories.Base;
using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookShop.Books.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BooksDb ctx;
        public IAsyncRepository<Author> BaseRepository { get; }

        public AuthorRepository(BooksDb ctx, IAsyncRepository<Author> _base)
        {
            this.ctx = ctx;
            BaseRepository = _base;
        }

        public async Task<bool> IsUniqueName(string name)
        {
            return !await ctx.Authors.AnyAsync(ent => ent.Name == name);
        }
    }
}
