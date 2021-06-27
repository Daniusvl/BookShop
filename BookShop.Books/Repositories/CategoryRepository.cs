using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Abstract.Repositories.Base;
using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookShop.Books.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BooksDb ctx;
        public IAsyncRepository<Category> BaseRepository { get; }

        public CategoryRepository(BooksDb ctx, IAsyncRepository<Category> _base)
        {
            this.ctx = ctx;
            BaseRepository = _base;
        }

        public async Task<bool> IsUniqueName(string name)
        {
            return !await ctx.Categories.AnyAsync(ent => ent.Name == name);
        }
    }
}
