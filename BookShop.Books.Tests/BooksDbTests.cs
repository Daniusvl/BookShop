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
            // arrange
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

            // act
            await repository.Create(author);

            // assert
            Assert.True(author.Id != 0);
            Assert.True(author.CreatedBy == $"{nameof(BooksDbTests)} {MethodBase.GetCurrentMethod().Name}");
        }

        [Fact]
        public async Task Create_And_GetById_Category_Test()
        {
            // arrange
            DbContextOptionsBuilder<BooksDb> options = new();
            options.UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name);

            Mock<ILoggedInUser> logged_in_user = new();
            logged_in_user.SetupGet(ex => ex.UserId)
                .Returns($"{nameof(BooksDbTests)} {MethodBase.GetCurrentMethod().Name}");

            using BooksDb context = new TestableBooksDb(options.Options, logged_in_user.Object);

            IAsyncRepository<Category> repository = new AsyncRepository<Category>(context);

            Category category = new()
            {
                Name = "Technical"
            };

            // act
            await repository.Create(category);

            // assert
            Assert.True(category.Id != 0);
            Assert.True(category.CreatedBy == $"{nameof(BooksDbTests)} {MethodBase.GetCurrentMethod().Name}");

            // act again
            Category result = await repository.GetById(category.Id);

            // assert again
            Assert.NotNull(result);
            Assert.Equal(category, result);
        }
    }
}
