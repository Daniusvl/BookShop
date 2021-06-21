using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Category.Commands.Update
{
    public static class UpdateCategory
    {
        public record Command(int Id, string Name) : IRequest;

        public class Handler : IRequestHandler<Command>
        {
            private readonly ICategoryRepository repository;
            private readonly ILogger<Handler> logger;

            public Handler(ICategoryRepository repository, ILogger<Handler> logger)
            {
                this.repository = repository;
                this.logger = logger;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                RequestValidator validator = new(repository);
                ValidationResult result = await validator.ValidateAsync(request);

                if(result.Errors.Count > 0)
                {
                    throw new ValidationException(result);
                }

                Domain.Entities.Category category = await repository.GetById(request.Id);

                if(category == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.Category), request.Id);
                }

                category.Name = request.Name;

                await repository.Update(category);

                logger.LogInformation($"{nameof(Domain.Entities.Category)} with Id: {category.Id} modified by {category.LastModifiedBy} at {category.DateLastModified}");

                return default;
            }
        }
    }
}
