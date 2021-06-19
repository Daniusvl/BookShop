using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Configuration;
using BookShop.Core.Exceptions;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.BookAuthor.Commands.Create
{
    public static class CreateBookAuthor
    {
        public record Command(string Name) : IRequest;

        public class Handler : IRequestHandler<Command>
        {
            private readonly IBookAuthorRepository repository;
            private readonly IConfiguration configuration;

            public Handler(IBookAuthorRepository repository, IConfiguration configuration)
            {
                this.repository = repository;
                this.configuration = configuration;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if(repository == null)
                {
                    throw new ServiceNullException(nameof(IProductRepository), nameof(Handler));
                }
                
                if(configuration == null)
                {
                    throw new ServiceNullException(nameof(IConfiguration), nameof(Handler));
                }

                if (configuration.IsDevelopment())
                {
                    if(request == null)
                    {
                        throw new ArgumentNullException(nameof(request));
                    }
                }

                RequestValidator validator = new (repository);
                ValidationResult result = await validator.ValidateAsync(request.Name);

                if (result.Errors.Count > 0)
                {
                    throw new ValidationException(result);
                }

                // TODO: Add validation and logging.

                Domain.Entities.BookAuthor bookAuthor = new()
                {
                    Name = request?.Name ?? string.Empty
                };

                await repository.Create(bookAuthor);

                return default;
            }
        }
    }
}
