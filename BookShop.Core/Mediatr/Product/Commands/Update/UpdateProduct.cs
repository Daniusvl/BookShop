using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
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
            private readonly ILogger<Handler> logger;

            public Handler(IProductRepository repository, ILogger<Handler> logger)
            {
                this.repository = repository;
                this.logger = logger;
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

                Domain.Entities.Product product = await repository.GetById(request.Product.Id);

                if(product == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.Product), request.Product.Id);
                }

                if(product.FilePath != request.Product.FilePath)
                {
                    result.Errors.Add(new ValidationFailure ( "FilePath"
                        , "You cannot change FilePath. If you want to update file it self, delete the old product and ad new one" ));
                    throw new ValidationException(result);
                }

                await repository.Update(request.Product);

                logger.LogInformation($"{nameof(Domain.Entities.Product)} with Id: {request.Product.Id} modified by {request.Product.LastModifiedBy} at {request.Product.DateLastModified}");

                return default;
            }
        }
    }
}
