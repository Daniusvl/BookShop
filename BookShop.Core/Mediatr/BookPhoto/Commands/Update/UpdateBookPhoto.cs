using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Configuration;
using BookShop.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.BookPhoto.Commands.Update
{
    public static class UpdateBookPhoto
    {
        public record Command(Domain.Entities.BookPhoto BookPhoto) : IRequest;

        public class Handler : IRequestHandler<Command>
        {
            private readonly IBookPhotoRepository repository;
            private readonly IConfiguration configuration;

            public Handler(IBookPhotoRepository repository, IConfiguration configuration)
            {
                this.repository = repository;
                this.configuration = configuration;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if (repository == null)
                {
                    throw new ServiceNullException(nameof(IBookPhotoRepository), nameof(Handler));
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

                    if(request.BookPhoto == null)
                    {
                        throw new ArgumentNullException(nameof(request.BookPhoto));
                    }
                }

                // TODO: Add validation and logging.

                Domain.Entities.BookPhoto bookPhoto = await repository.GetById(request.BookPhoto.Id);

                if(bookPhoto == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.BookPhoto), request.BookPhoto.Id);
                }

                await repository.Update(request.BookPhoto);

                return default;
            }
        }
    }
}
