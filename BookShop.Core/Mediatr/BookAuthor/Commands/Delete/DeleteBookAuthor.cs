using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Configuration;
using BookShop.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.BookAuthor.Commands.Delete
{
    public static class DeleteBookAuthor
    {
        public record Command(int Id) : IRequest;

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
                }

                // TODO: Add validation and logging.

                Domain.Entities.BookAuthor bookAuthor = await repository.GetById(request.Id);

                if(bookAuthor == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.BookAuthor), request.Id);
                }

                await repository.Delete(bookAuthor);

                return default;
            }
        }
    }
}
