using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Configuration;
using BookShop.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Category.Commands.Create
{
    public static class CreateCategory
    {
        public record Command(string Name) : IRequest;

        public class Handler : IRequestHandler<Command>
        {
            private readonly ICategoryRepository repository;
            private readonly IConfiguration configuration;

            public Handler(ICategoryRepository repository, IConfiguration configuration)
            {
                this.repository = repository;
                this.configuration = configuration;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if(repository == null)
                {
                    throw new ServiceNullException(nameof(ICategoryRepository), nameof(Handler));
                }

                if(configuration == null)
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

                Domain.Entities.Category category = new()
                {
                    Name = request?.Name ?? string.Empty
                };

                await repository.Create(category);

                return default;
            }
        }
    }
}
