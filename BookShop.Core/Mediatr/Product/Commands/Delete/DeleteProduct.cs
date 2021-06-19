using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using FluentValidation.Results;
using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Product.Commands.Delete
{
    public static class DeleteProduct
    {
        public record Command(int Id) : IRequest;

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

                RequestValidator validator = new();
                ValidationResult result = await validator.ValidateAsync(request);

                if(result.Errors.Count > 0)
                {
                    throw new ValidationException(result);
                }

                // TODO: Add validation and logging.

                Domain.Entities.Product product = await repository.GetById(request.Id);

                if(product == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.Product), request.Id);
                }

                File.Delete(product.FilePath);

                await repository.Delete(product);

                return default;
            }
        }
    }
}
