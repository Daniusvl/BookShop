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

            // assert
            Assert.NotNull(result);
            Assert.True(File.Exists(result.FilePath));

            // delete created junk file and directories
            File.Delete(result.FilePath);
            Directory.Delete($@"Photos\{command.ProductName}");
            Directory.Delete("Photos");
        }
    }
}
