using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Configuration;
using BookShop.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Category.Commands.Update
{
    public static class UpdateCategory
    {
        public record Command(Domain.Entities.Category Category) : IRequest;

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
                if (repository == null)
                {
                    throw new ServiceNullException(nameof(ICategoryRepository), nameof(Handler));
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

                    if(request.Category == null)
                    {
                        throw new ArgumentNullException(nameof(request.Category));
                    }
                }

                // TODO: Add validation and logging.

                Domain.Entities.Category category = await repository.GetById(request.Category.Id);

                if(category == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.Category), request.Category.Id);
                }

                await repository.Update(category);

                return default;
            }
        }
    }
}
