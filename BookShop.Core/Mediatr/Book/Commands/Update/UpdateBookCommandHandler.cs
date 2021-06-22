using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Book.Commands.Update
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, BookModel>
    {
        private readonly IBookRepository repository;
        private readonly IPhotoRepository photoRepository;
        private readonly IAuthorRepository authorRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;
        private readonly ILogger<UpdateBookCommandHandler> logger;

        public UpdateBookCommandHandler(IBookRepository repository, IPhotoRepository photoRepository, IAuthorRepository authorRepository,
            ICategoryRepository categoryRepository, IMapper mapper, ILogger<UpdateBookCommandHandler> logger)
        {
            this.repository = repository;
            this.photoRepository = photoRepository;
            this.authorRepository = authorRepository;
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<BookModel> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            UpdateBookRequestValidator validator = new(repository);
            ValidationResult result = await validator.ValidateAsync(request);

            if (result.Errors.Count > 0)
            {
                throw new ValidationException(result);
            }

            Domain.Entities.Book book = await repository.GetById(request.Id);

            if (book == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Book), request.Id);
            }

            book.Name = request.Name;
            book.Description = request.Description;
            book.Price = request.Price;
            book.Hidden = request.Hidden;
            book.DateReleased = request.DateReleased;

            foreach (int id in request.BookPhotoIds ?? new List<int>())
            {
                Domain.Entities.Photo photo = await photoRepository.GetById(id);

                if (photo == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.Photo), id);
                }

                book.Photos.Add(photo);
            }

            Domain.Entities.Author author = await authorRepository.GetById(request.AuthorId);

            if (author == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Author), request.AuthorId);
            }

            book.Author = author;

            Domain.Entities.Category category = await categoryRepository.GetById(request.CategoryId);

            if (categoryRepository == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Category), request.CategoryId);
            }

            book.Category = category;

            await repository.Update(book);

            logger.LogInformation($"{nameof(Domain.Entities.Book)} with Id: {book.Id} modified by {book.LastModifiedBy} at {book.DateLastModified}");

            return mapper.Map<Domain.Entities.Book, BookModel>(book);
        }
    }
}
