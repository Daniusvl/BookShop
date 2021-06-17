using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Configuration;
using BookShop.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.BookAuthor.Commands.Update
{
    public static class UpdateBookAuthor
    {
        public record Command(Domain.Entities.BookAuthor BookAuthor) : IRequest;

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
                if (repository == null)
                {
                    throw new ServiceNullException(nameof(IProductRepository), nameof(Handler));
                }

                if (configuration == null)
                {
                    throw new ServiceNullException(nameof(IConfiguration), nameof(Handler));
                }

                if (configuration.IsDevelopment())
                {
                    if (request == null)
                    {
                        throw new ArgumentNullException(nameof(request));
                    }

                    if (request.BookAuthor == null)
                    {
                        throw new ArgumentNullException(nameof(request.BookAuthor));
                    }
                }

                // TODO: Add validation and logging.

                await repository.Update(request?.BookAuthor);

                return default;
            }
        }
    }
}
