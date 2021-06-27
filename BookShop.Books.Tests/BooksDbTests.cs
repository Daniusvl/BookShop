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

            using BooksDb context = new TestableBooksDb(options.Options, null, logged_in_user.Object);

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

            using BooksDb context = new TestableBooksDb(options.Options,null, logged_in_user.Object);

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

        [Fact]
        public async Task Create_And_Delete_Product_Test()
        {
            // arrange
            DbContextOptionsBuilder<BooksDb> options = new();
            options.UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name);

            Mock<ILoggedInUser> logged_in_user = new();
            logged_in_user.SetupGet(ex => ex.UserId)
                .Returns($"{nameof(BooksDbTests)} {MethodBase.GetCurrentMethod().Name}");

            using BooksDb context = new TestableBooksDb(options.Options, null, logged_in_user.Object);

            IAsyncRepository<Book> repository = new AsyncRepository<Book>(context);

            Book book = new()
            {
                Name = "C# in Depth"
            };

            // act
            await repository.Create(book);

            // assert
            Assert.True(book.Id != 0);

            // act again
            await repository.Delete(book);
            Book result = await repository.GetById(book.Id);

            // assert again
            Assert.Null(result);
        }

        [Fact]
        public async Task Create_And_Update_Photo_Test()
        {
            // arrange
            DbContextOptionsBuilder<BooksDb> options = new();
            options.UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name);

            Mock<ILoggedInUser> logged_in_user = new();
            logged_in_user.SetupGet(ex => ex.UserId)
                .Returns($"{nameof(BooksDbTests)} {MethodBase.GetCurrentMethod().Name}");

            using BooksDb context = new TestableBooksDb(options.Options, null, logged_in_user.Object);

            IAsyncRepository<Photo> repository = new AsyncRepository<Photo>(context);

            Photo photo = new()
            {
                FilePath = "Cyberpunk 2077 is shit"
            };

            string previous_value = photo.FilePath;

            // act
            await repository.Create(photo);

            // assert
            Assert.True(photo.Id != 0);

            // act again
            photo.FilePath = "But I didnt played it";
            await repository.Update(photo);
            Photo result = await repository.GetById(photo.Id);

            // assert again
            Assert.Equal(photo, result);
            Assert.False(result.FilePath == previous_value);
        }
    }
}
