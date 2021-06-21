using BookShop.Core.Abstract;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Category.Commands.Delete
{
    public static class DeleteCategory
    {
        public record Command(int Id) : IRequest;

        public class Handler : IRequestHandler<Command>
        {
            private readonly ICategoryRepository repository;
            private readonly ILoggedInUser loggedInUser;
            private readonly ILogger<Handler> logger;

            public Handler(ICategoryRepository repository, ILoggedInUser loggedInUser, ILogger<Handler> logger)
            {
                this.repository = repository;
                this.loggedInUser = loggedInUser;
                this.logger = logger;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if (repository == null)
                {
                    throw new ServiceNullException(nameof(ICategoryRepository), nameof(Handler));
                }

                RequestValidator validator = new();
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

                await repository.Delete(category);

                logger.LogInformation($"{nameof(Domain.Entities.Category)} with Id: {request.Id} deleted by {loggedInUser.UserId} at {DateTime.Now}");

                return default;
            }
        }
    }
}
