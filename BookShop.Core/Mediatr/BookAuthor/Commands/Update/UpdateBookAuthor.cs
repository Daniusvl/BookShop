using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.BookAuthor.Commands.Update
{
    public static class UpdateBookAuthor
    {
        public record Command(int Id, string Name) : IRequest;

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
                if (repository == null)
                {
                    throw new ServiceNullException(nameof(IBookAuthorRepository), nameof(Handler));
                }

                RequestValidator validator = new(repository);
                ValidationResult result = await validator.ValidateAsync(request);
                
                if(result.Errors.Count > 0)
                {
                    throw new ValidationException(result);
                }

                Domain.Entities.BookAuthor bookAuthor = await repository.GetById(request.Id);

                if(bookAuthor == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.BookAuthor), request.Id);
                }

                bookAuthor.Name = request.Name;

                await repository.Update(bookAuthor);

                logger.LogInformation($"{nameof(Domain.Entities.BookAuthor)} with Id: {bookAuthor.Id} updated by {bookAuthor.LastModifiedBy} at {bookAuthor.DateLastModified}");


                return default;
            }
        }
    }
}
