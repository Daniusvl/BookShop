using BookShop.Core.Abstract;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.BookAuthor.Commands.Delete
{
    public static class DeleteBookAuthor
    {
        public record Command(int Id) : IRequest;

        public class Handler : IRequestHandler<Command>
        {
            private readonly IBookAuthorRepository repository;
            private readonly ILoggedInUser loggedInUser;
            private readonly ILogger<Handler> logger;

            public Handler(IBookAuthorRepository repository, ILoggedInUser loggedInUser, ILogger<Handler> logger)
            {
                this.repository = repository;
                this.loggedInUser = loggedInUser;
                this.logger = logger;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if (repository == null)
                {
                    throw new ServiceNullException(nameof(IBookAuthorRepository), nameof(Handler));
                }

                RequestValidator validatiors = new();
                ValidationResult result = await validatiors.ValidateAsync(request);

                if(result.Errors.Count > 0)
                {
                    throw new ValidationException(result);
                }

                Domain.Entities.BookAuthor bookAuthor = await repository.GetById(request.Id);

                if(bookAuthor == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.BookAuthor), request.Id);
                }

                await repository.Delete(bookAuthor);

                logger.LogInformation($"{nameof(Domain.Entities.BookAuthor)} with Id: {request.Id} deleted by {loggedInUser.UserId} at {DateTime.Now}");

                return default;
            }
        }
    }
}
