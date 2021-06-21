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

namespace BookShop.Core.Mediatr.Product.Commands.Create
{
    public static class CreateProduct
    {
        public record Command(Domain.Entities.Product Product, IList<byte> Bytes) : IRequest;

        public class Handler : IRequestHandler<Command>
        {
            private readonly IProductRepository repository;
            private readonly ILogger<Handler> logger;

            public Handler(IProductRepository repository, ILogger<Handler> logger)
            {
                this.repository = repository;
                this.logger = logger;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if(repository == null)
                {
                    throw new ServiceNullException(nameof(IProductRepository), nameof(Handler));
                }

                RequestValidator validator = new(repository);
                ValidationResult result = await validator.ValidateAsync(request);

                if(result.Errors.Count > 0)
                {
                    throw new ValidationException(result);
                }

                string path = Directory.GetCurrentDirectory() + $@"\Books\{Guid.NewGuid()}.pdf";

                int i = 0;
                while (File.Exists(path))
                {
                    path = Directory.GetCurrentDirectory() + $@"\Books\{Guid.NewGuid()}.pdf";
                    i++;
                }

                if(i >= 5)
                {
                    logger.LogWarning("Product file name generation takes more than 4 iterations");
                }

                await File.WriteAllBytesAsync(path, request.Bytes.ToArray());

                request.Product.FilePath = path;

                await repository.Create(request.Product);

                logger.LogInformation($"{nameof(Domain.Entities.Product)} with Id: {request.Product.Id} created by {request.Product.CreatedBy} at {request.Product.DateCreated}");

                return default;
            }
        }
    }
}
