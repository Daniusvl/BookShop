using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using FluentValidation.Results;
using MediatR;
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

            public Handler(IBookPhotoRepository repository, IProductRepository productRepository)
            {
                this.repository = repository;
                this.productRepository = productRepository;
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

                // TODO: Add validation and logging.
                
                if(!Directory.Exists(Directory.GetCurrentDirectory() + @$"\Photos\{request.ProductName}"))
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + @$"\Photos\{request.ProductName}");
                }

                string path = Directory.GetCurrentDirectory() + @$"\Photos\{request.ProductName}\{Guid.NewGuid()}.png";

                while (File.Exists(path))
                {
                    path = Directory.GetCurrentDirectory() + @$"\Photos\{request.ProductName}\{Guid.NewGuid()}.png";
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
