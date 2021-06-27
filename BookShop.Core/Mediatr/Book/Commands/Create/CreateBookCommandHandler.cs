using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Book.Commands.Create
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, BookModel>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<CreateBookCommandHandler> logger;

        public CreateBookCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateBookCommandHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<BookModel> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            CreateBookRequestValidator validator = new(unitOfWork.BookRepository);
            ValidationResult result = await validator.ValidateAsync(request);

            if (result.Errors.Count > 0)
            {
                throw new ValidationException(result);
            }

            string path = Directory.GetCurrentDirectory() + $@"\Books\{Guid.NewGuid()}.pdf";

            int i = 0;
            while (File.Exists(path))
            {
                path = Directory.GetCurrentDirectory() + $@"\Books\{Guid.NewGuid()}.pdf";
                i++;
            }

            if (i >= 5)
            {
                logger.LogWarning("Product file name generation takes more than 4 iterations");
            }

            Domain.Entities.Book book = new Domain.Entities.Book
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                FilePath = path,
                Hidden = request.Hidden,
                DateReleased = request.DateReleased
            };

            Domain.Entities.Author author = await unitOfWork.AuthorRepository.BaseRepository.GetById(request.AuthorId);

            if (author == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Author), request.AuthorId);
            }

            book.AuthorId = author.Id;
            book.Author = author;

            Domain.Entities.Category category = await unitOfWork.CategoryRepository.BaseRepository.GetById(request.CategoryId);

            if (category == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Category), request.CategoryId);
            }

            book.CategoryId = category.Id;
            book.Category = category;

            await unitOfWork.BookRepository.BaseRepository.Create(book);

            logger.LogInformation($"{nameof(Domain.Entities.Book)} with Id: {book.Id} created by {book.CreatedBy} at {book.DateCreated}");

            await File.WriteAllBytesAsync(path, request.Bytes.ToArray());

            return mapper.Map<Domain.Entities.Book, BookModel>(book);
        }
    }
}
