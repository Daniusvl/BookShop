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

namespace BookShop.Core.Mediatr.BookPhoto.Commands.Create
{
    public static class CreateBookPhoto
    {
        public record Command(string ProductName, IList<byte> FileBytes) : IRequest;

        public class Handler : IRequestHandler<Command>
        {
            private readonly IBookPhotoRepository repository;
            private readonly IProductRepository productRepository;
            private readonly ILogger<Handler> logger;

            public Handler(IBookPhotoRepository repository, IProductRepository productRepository, ILogger<Handler> logger)
            {
                this.repository = repository;
                this.productRepository = productRepository;
                this.logger = logger;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if(repository == null)
                {
                    throw new ServiceNullException(nameof(IBookPhotoRepository), nameof(Handler));
                }

                RequestValidator validator = new(productRepository);
                ValidationResult result = await validator.ValidateAsync(request);

                if(result.Errors.Count > 0)
                {
                    throw new ValidationException(result);
                }
                
                if(!Directory.Exists(Directory.GetCurrentDirectory() + @$"\Photos\{request.ProductName}"))
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + @$"\Photos\{request.ProductName}");
                }

                string path = Directory.GetCurrentDirectory() + @$"\Photos\{request.ProductName}\{Guid.NewGuid()}.png";

                int i = 0;
                while (File.Exists(path))
                {
                    path = Directory.GetCurrentDirectory() + @$"\Photos\{request.ProductName}\{Guid.NewGuid()}.png";
                    i++;
                }

                if(i >= 5)
                {
                    logger.LogWarning("Photo file name generation takes more than 4 iterations");
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
