using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Product.Commands.Update
{
    public static class UpdateProduct
    {
        public record Command(int Id, string Name, string Description, decimal Price, bool Hidden, DateTime DateReleased,
            IList<int> BookPhotoIds, int AuthorId, int CategoryId) : IRequest;

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
                if (repository == null)
                {
                    throw new ServiceNullException(nameof(IProductRepository), nameof(Handler));
                }

                RequestValidator validator = new(repository);
                ValidationResult result = await validator.ValidateAsync(request);

                if (result.Errors.Count > 0) 
                {
                    throw new ValidationException(result);
                }

                Domain.Entities.Product product = await repository.GetById(request.Id);

                if(product == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.Product), request.Id);
                }

                product.Name = request.Name;
                product.Description = request.Description;
                product.Price = request.Price;
                product.Hidden = request.Hidden;
                product.DateReleased = request.DateReleased;

                foreach (int id in request.BookPhotoIds ?? new List<int>())
                {
                    Domain.Entities.BookPhoto photo = await photoRepository.GetById(id);

                    if (photo == null)
                    {
                        throw new NotFoundException(nameof(Domain.Entities.BookPhoto), id);
                    }

                    product.Photos.Add(photo);
                }

                Domain.Entities.BookAuthor author = await authorRepository.GetById(request.AuthorId);

                if (author == null)
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

                await repository.Update(product);

                logger.LogInformation($"{nameof(Domain.Entities.Product)} with Id: {product.Id} modified by {product.LastModifiedBy} at {product.DateLastModified}");

                return default;
            }
        }
    }
}
