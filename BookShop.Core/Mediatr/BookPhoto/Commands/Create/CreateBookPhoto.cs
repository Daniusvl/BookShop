using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Configuration;
using BookShop.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.BookPhoto.Commands.Create
{
    public static class CreateBookPhoto
    {
        public record Command(string ProductName, IList<byte> FileBytes) : IRequest;

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
                if(repository == null)
                {
                    throw new ServiceNullException(nameof(IBookPhotoRepository), nameof(Handler));
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

                    if(request.FileBytes == null)
                    {
                        throw new ArgumentNullException(nameof(request.FileBytes));
                    }

                    if (request.ProductName == null)
                    {
                        throw new ArgumentNullException(nameof(request.FileBytes));
                    }
                }

                // TODO: Add validation and logging.
                
                string path = Directory.GetCurrentDirectory() + @$"\{request.ProductName}\{Guid.NewGuid()}.png";

                while (File.Exists(path))
                {
                    path = Directory.GetCurrentDirectory() + @$"\{request.ProductName}\{Guid.NewGuid()}.png";
                }

                await File.WriteAllBytesAsync(path, request.FileBytes.ToArray());

                Domain.Entities.BookPhoto bookPhoto = new()
                {
                    FilePath = path
                };

                await repository.Create(bookPhoto);

                return default;
            }
        }
    }
}
