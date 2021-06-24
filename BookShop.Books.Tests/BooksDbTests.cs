using BookShop.Books.Repositories;
using BookShop.Core.Abstract;
using BookShop.Core.Abstract.Repositories.Base;
using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace BookShop.Books.Tests
{
    public class BooksDbTests
    {
        [Fact]
        public async Task Create_Author_Test()
        {
            DbContextOptionsBuilder<BooksDb> options = new();
            options.UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name);

            Mock<ILoggedInUser> logged_in_user = new();
            logged_in_user.SetupGet(ex => ex.UserId)
                .Returns($"{nameof(BooksDbTests)} {MethodBase.GetCurrentMethod().Name}");

            using BooksDb context = new TestableBooksDb(options.Options, logged_in_user.Object);

            IAsyncRepository<Author> repository = new AsyncRepository<Author>(context);

            Author author = new()
            {
                Name = "Jeffrey Richter"
            };

            await repository.Create(author);

            Assert.True(author.Id != 0);
            Assert.True(author.CreatedBy == $"{nameof(BooksDbTests)} {MethodBase.GetCurrentMethod().Name}");
        }
    }
}
