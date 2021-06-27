using BookShop.Core.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BookShop.Books.Tests
{
    public class TestableBooksDb : BooksDb
    {
        public TestableBooksDb(DbContextOptions<BooksDb> options, IConfiguration? configuration, ILoggedInUser loggedInUser)
            : base(options, configuration, loggedInUser) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}
