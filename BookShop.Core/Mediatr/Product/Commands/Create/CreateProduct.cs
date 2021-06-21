using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Product.Commands.Create
{
    public static class CreateProduct
    {
        public record Command(string Name, string Description, decimal Price, bool Hidden, DateTime DateReleased,
            IList<int> BookPhotoIds, int AuthorId, int CategoryId, IList<byte> Bytes) : IRequest;

        public class Handler : IRequestHandler<Command>
        {
            private readonly IProductRepository repository;
            private readonly IBookPhotoRepository photoRepository;
            private readonly IBookAuthorRepository authorRepository;
            private readonly ICategoryRepository categoryRepository;
            private readonly ILogger<Handler> logger;

            public Handler(IProductRepository repository, IBookPhotoRepository photoRepository, IBookAuthorRepository authorRepository, 
                ICategoryRepository categoryRepository, ILogger<Handler> logger)
            {
                this.repository = repository;
                this.photoRepository = photoRepository;
                this.authorRepository = authorRepository;
                this.categoryRepository = categoryRepository;
                this.logger = logger;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if(repository == null)
                {
                    throw new ServiceNullException(nameof(IProductRepository), nameof(Handler));
                }

                RequestValidator validator = new(repository);
                ValidationResult result = await validator.ValidateAsync(request);

                if(result.Errors.Count > 0)
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

                if(i >= 5)
                {
                    logger.LogWarning("Product file name generation takes more than 4 iterations");
                }

                await File.WriteAllBytesAsync(path, request.Bytes.ToArray());

                Domain.Entities.Product product = new Domain.Entities.Product
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
                    Domain.Entities.BookPhoto photo = await photoRepository.GetById(id);

                    if(photo == null)
                    {
                        throw new NotFoundException(nameof(Domain.Entities.BookPhoto), id);
                    }

                    product.Photos.Add(photo);
                }

                Domain.Entities.BookAuthor author = await authorRepository.GetById(request.AuthorId);

                if(author == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.BookAuthor), request.AuthorId);
                }

                product.Author = author;

                Domain.Entities.Category category = await categoryRepository.GetById(request.CategoryId);

                if (categoryRepository == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.Category), request.CategoryId);
                }

                product.Category = category;

                await repository.Create(product);

                logger.LogInformation($"{nameof(Domain.Entities.Product)} with Id: {product.Id} created by {product.CreatedBy} at {product.DateCreated}");

                return default;
            }
        }
    }
}
