using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Configuration;
using BookShop.Core.Exceptions;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Product.Commands.Update
{
    public static class UpdateProduct
    {
        public record Command(Domain.Entities.Product Product) : IRequest;

        public class Handler : IRequestHandler<Command>
        {
            private readonly IProductRepository repository;

            public Handler(IProductRepository repository)
            {
                this.repository = repository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if (repository == null)
                {
                    throw new ServiceNullException(nameof(IProductRepository), nameof(Handler));
                }

                RequestValidator validator = new(repository);
                ValidationResult result = await validator.ValidateAsync(request);

                if (result.Errors.Count > 0) 
                {
                    throw new ValidationException(result);
                }

                // TODO: Add validation and logging.

                Domain.Entities.Product product = await repository.GetById(request.Product.Id);

                if(product == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.Product), request.Product.Id);
                }

                await repository.Update(request.Product);

                return default;
            }
        }
    }
}
