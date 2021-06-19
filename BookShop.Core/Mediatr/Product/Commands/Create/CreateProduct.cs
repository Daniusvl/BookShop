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

namespace BookShop.Core.Mediatr.Product.Commands.Create
{
    public static class CreateProduct
    {
        public record Command(Domain.Entities.Product Product, IList<byte> Bytes) : IRequest;

        public class Handler : IRequestHandler<Command>
        {
            private readonly IProductRepository repository;

            public Handler(IProductRepository repository)
            {
                this.repository = repository;
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

                // TODO: Add validation and logging.

                string path = Directory.GetCurrentDirectory() + $@"\Books\{Guid.NewGuid()}.pdf";

                while (File.Exists(path))
                {
                    path = Directory.GetCurrentDirectory() + $@"\Books\{Guid.NewGuid()}.pdf";
                }

                await File.WriteAllBytesAsync(path, request.Bytes.ToArray());

                request.Product.FilePath = path;

                await repository.Create(request.Product);

                return default;
            }
        }
    }
}
