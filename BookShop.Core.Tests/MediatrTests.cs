using Xunit;
using Moq;
using BookShop.Core.Abstract.Repositories;
using BookShop.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AutoMapper;
using BookShop.Core.Models;
using System;
using BookShop.Core.Mediatr.Photo.Commands.Create;
using System.Collections.Generic;
using System.IO;
using BookShop.Core.Mediatr.Author.Commands.Update;
using BookShop.Core.Exceptions;
using BookShop.Core.Abstract;
using BookShop.Core.Mediatr.Book.Commands.Delete;

namespace BookShop.Core.Tests
{
    public class MediatrTests
    {
        [Fact]
        public async Task Photo_File_Creation_Test()
        {
            // arange
            Mock<IPhotoRepository> photo_repository = new();

            Mock<IBookRepository> book_repository = new();
            book_repository.Setup(ex => ex.ContainsWithName(It.IsAny<string>()))
                .Returns(true);

            Mock<ILogger<CreatePhotoCommandHandler>> logger = new();

            Mock<IMapper> mapper = new Mock<IMapper>();
            Func<Photo, PhotoModel> func = photo => new PhotoModel() { FilePath = photo.FilePath };
            mapper.Setup(ex => ex.Map<Photo, PhotoModel>(It.IsAny<Photo>()))
                .Returns(func);

            CreatePhotoCommand command = new() { ProductName = "ProductName", FileBytes = new List<byte> { 0x00, 0x00 } };
            CreatePhotoCommandHandler handler = new(photo_repository.Object, book_repository.Object, mapper.Object, logger.Object);

            // act
            PhotoModel result = await handler.Handle(command, default);

            bool file_exists = File.Exists(result.FilePath);

            // delete created junk file and directories
            File.Delete(result.FilePath);
            Directory.Delete($@"Photos\{command.ProductName}");
            Directory.Delete("Photos");

            // assert
            Assert.NotNull(result);
            Assert.True(file_exists);
        }

        [Fact]
        public async Task Update_Author_Throw_Exception_If_Id_Not_Found_Test()
        {
            // arange
            Mock<IAuthorRepository> repository = new();
            repository.Setup(ex => ex.GetById(It.IsAny<int>()))
                .Returns(Task.FromResult<Author>(null));
            repository.Setup(ex => ex.IsUniqueName(It.IsAny<string>()))
                .Returns(true);

            Mock<IMapper> mapper = new();

            Mock<ILogger<UpdateAuthorCommandHandler>> logger = new();

            UpdateAuthorCommand command = new() { Id = 1337, Name = "Dont care" };
            UpdateAuthorCommandHandler handler = new(repository.Object, mapper.Object, logger.Object);

            // act, assert
            await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Delete_Book_File_Test()
        {
            // arrange
            string path = $@"{Directory.GetCurrentDirectory()}\Books";
            Directory.CreateDirectory(path);
            string file_path = $@"{path}\file.pdf";
            File.WriteAllBytes(file_path, new byte[] { 0, 0, 0, 0 });

            Mock<IBookRepository> repository = new();
            repository.Setup(ex => ex.GetById(It.IsAny<int>()))
                .Returns(Task.FromResult(new Book { FilePath = file_path }));

            Mock<ILoggedInUser> logged_in_user = new();

            Mock<ILogger<DeleteBookCommandHandler>> logger = new();

            DeleteBookCommand command = new(1337);

            DeleteBookCommandHandler handler = new(repository.Object, logged_in_user.Object, logger.Object);

            // act
            await handler.Handle(command, default);

            bool file_exists = File.Exists(file_path);

            // delete created files
            if (file_exists)
                File.Delete(file_path);
            Directory.Delete(path);

            // assert
            Assert.False(file_exists);
        }
    }
}
