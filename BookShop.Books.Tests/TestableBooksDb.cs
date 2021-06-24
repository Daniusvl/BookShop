using BookShop.Core.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Books.Tests
{
    public class TestableBooksDb : BooksDb
    {
        public TestableBooksDb(DbContextOptions<BooksDb> options, ILoggedInUser loggedInUser)
            : base(options, loggedInUser) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}
