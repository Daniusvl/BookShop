using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Configuration;
using BookShop.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.BookPhoto.Commands.Delete
{
    public static class DeleteBookPhoto
    {
        public record Command(int Id) : IRequest;

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
                }

                // TODO: Add validation and logging.

                Domain.Entities.BookPhoto bookPhoto = await repository.GetById(request.Id);

                if(bookPhoto == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.BookPhoto), request.Id);
                }

                File.Delete(bookPhoto.FilePath);

                await repository.Delete(bookPhoto);

                return default;
            }
        }
    }
}
