using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.BookAuthor.Commands.Create
{
    public static class CreateBookAuthor
    {
        public record Command(string Name) : IRequest;

        public class Handler : IRequestHandler<Command>
        {
            private readonly IBookAuthorRepository repository;
            private readonly ILogger<Handler> logger;

            public Handler(IBookAuthorRepository repository, ILogger<Handler> logger)
            {
                this.repository = repository;
                this.logger = logger;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if(repository == null)
                {
                    throw new ServiceNullException(nameof(IBookAuthorRepository), nameof(Handler));
                }

                RequestValidator validator = new (repository);
                ValidationResult result = await validator.ValidateAsync(request);

                if (result.Errors.Count > 0)
                {
                    throw new ValidationException(result);
                }

                Domain.Entities.BookAuthor bookAuthor = new()
                {
                    Name = request.Name
                };

                await repository.Create(bookAuthor);

                logger.LogInformation($"{nameof(Domain.Entities.BookAuthor)} with Id: {bookAuthor.Id} created by {bookAuthor.CreatedBy} at {bookAuthor.DateCreated}");

                return default;
            }
        }
    }
}
