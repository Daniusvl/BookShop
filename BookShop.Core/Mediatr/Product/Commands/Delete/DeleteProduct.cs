using BookShop.Core.Abstract;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
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
            private readonly ILoggedInUser loggedInUser;
            private readonly ILogger<Handler> logger;

            public Handler(IProductRepository repository, ILoggedInUser loggedInUser, ILogger<Handler> logger)
            {
                this.repository = repository;
                this.loggedInUser = loggedInUser;
                this.logger = logger;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                RequestValidator validator = new();
                ValidationResult result = await validator.ValidateAsync(request);

                if(result.Errors.Count > 0)
                {
                    throw new ValidationException(result);
                }

                Domain.Entities.Product product = await repository.GetById(request.Id);

                if(product == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.Product), request.Id);
                }

                File.Delete(product.FilePath);

                await repository.Delete(product);

                logger.LogInformation($"{nameof(Domain.Entities.Product)} with Id: {request.Id} deleted by {loggedInUser.UserId} at {DateTime.Now}");

                return default;
            }
        }
    }
}
