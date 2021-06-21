using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Book.Commands.Create
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, BookModel>
    {
        private readonly IBookRepository repository;
        private readonly IPhotoRepository photoRepository;
        private readonly IAuthorRepository authorRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;
        private readonly ILogger<CreateBookCommandHandler> logger;

        public CreateBookCommandHandler(IBookRepository repository, IPhotoRepository photoRepository, IAuthorRepository authorRepository,
            ICategoryRepository categoryRepository, IMapper mapper, ILogger<CreateBookCommandHandler> logger)
        {
            this.repository = repository;
            this.photoRepository = photoRepository;
            this.authorRepository = authorRepository;
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public IPhotoRepository PhotoRepository => photoRepository;

        public async Task<BookModel> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            CreateBookRequestValidator validator = new(repository);
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

            await File.WriteAllBytesAsync(path, request.Bytes.ToArray());

            Domain.Entities.Book product = new Domain.Entities.Book
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                FilePath = path,
                Hidden = request.Hidden,
                DateReleased = request.DateReleased
            };

            foreach (int id in request.BookPhotoIds ?? new List<int>())
            {
                Domain.Entities.Photo photo = await PhotoRepository.GetById(id);

                if (photo == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.Photo), id);
                }

                product.Photos.Add(photo);
            }

            Domain.Entities.Author author = await authorRepository.GetById(request.AuthorId);

            if (author == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Author), request.AuthorId);
            }

            product.Author = author;

            Domain.Entities.Category category = await categoryRepository.GetById(request.CategoryId);

            if (categoryRepository == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Category), request.CategoryId);
            }

            product.Category = category;

            await repository.Create(product);

            logger.LogInformation($"{nameof(Domain.Entities.Book)} with Id: {product.Id} created by {product.CreatedBy} at {product.DateCreated}");

            return mapper.Map<Domain.Entities.Book, BookModel>(product);
        }
    }
}
