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
using BookShop.Core.Abstract.Repositories.Base;

namespace BookShop.Core.Tests
{
    public class MediatrTests
    {
        [Fact]
        public async Task Photo_File_Creation_Test()
        {
            // arange
            Mock<IAsyncRepository<Photo>> base_photo_repository = new();

            Mock<IPhotoRepository> photo_repository = new();
            photo_repository.SetupGet(repo => repo.BaseRepository).Returns(base_photo_repository.Object);

            Mock<IBookRepository> book_repository = new();
            book_repository.Setup(ex => ex.ContainsWithName(It.IsAny<string>()))
                .Returns(Task.FromResult(true));
            book_repository.Setup(ex => ex.GetByName(It.IsAny<string>()))
                .Returns(Task.FromResult(new Book { Id = 1 }));

            Mock<IUnitOfWork> unit_of_work = new();
            unit_of_work.SetupGet(ex => ex.BookRepository)
                .Returns(book_repository.Object);
            unit_of_work.SetupGet(ex => ex.PhotoRepository)
                .Returns(photo_repository.Object);

            Mock<ILogger<CreatePhotoCommandHandler>> logger = new();

            Mock<IMapper> mapper = new Mock<IMapper>();
            Func<Photo, PhotoModel> func = photo => new PhotoModel() { FileBytes = File.ReadAllBytes(photo.FilePath) };
            mapper.Setup(ex => ex.Map<Photo, PhotoModel>(It.IsAny<Photo>()))
                .Returns(func);

            CreatePhotoCommand command = new() { ProductName = "ProductName", FileBytes = new List<byte> { 0x99, 0x99 } };
            CreatePhotoCommandHandler handler = new(unit_of_work.Object, mapper.Object, logger.Object);

            // act
            PhotoModel result = await handler.Handle(command, default);

            bool file_exists = result.FileBytes != null && result.FileBytes.Count > 0;

            // delete created junk file and directories
            foreach (FileInfo file_info in new DirectoryInfo(Directory.GetCurrentDirectory() + @"\Photos\ProductName").GetFiles())
            {
                file_info.Delete();
            }
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
            repository.Setup(ex => ex.BaseRepository.GetById(It.IsAny<int>()))
                .Returns(Task.FromResult<Author>(null));
            repository.Setup(ex => ex.IsUniqueName(It.IsAny<string>()))
                .Returns(Task.FromResult(true));

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
            repository.Setup(ex => ex.BaseRepository.GetById(It.IsAny<int>()))
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
