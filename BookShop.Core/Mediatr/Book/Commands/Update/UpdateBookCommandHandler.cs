using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Book.Commands.Update
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, BookModel>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<UpdateBookCommandHandler> logger;

        public UpdateBookCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateBookCommandHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<BookModel> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            UpdateBookRequestValidator validator = new(unitOfWork.BookRepository);
            ValidationResult result = await validator.ValidateAsync(request);

            if (result.Errors.Count > 0)
            {
                throw new ValidationException(result);
            }

            Domain.Entities.Book book = await unitOfWork.BookRepository.BaseRepository.GetById(request.Id);

            if (book == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Book), request.Id);
            }

            book.Name = request.Name;
            book.Description = request.Description;
            book.Price = request.Price;
            book.Hidden = request.Hidden;
            book.DateReleased = request.DateReleased;

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

            await unitOfWork.BookRepository.BaseRepository.Update(book);

            logger.LogInformation($"{nameof(Domain.Entities.Book)} with Id: {book.Id} modified by {book.LastModifiedBy} at {book.DateLastModified}");

            return mapper.Map<Domain.Entities.Book, BookModel>(book);
        }
    }
}
