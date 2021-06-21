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

namespace BookShop.Core.Mediatr.BookPhoto.Commands.Delete
{
    public static class DeleteBookPhoto
    {
        public record Command(int Id) : IRequest;

        public class Handler : IRequestHandler<Command>
        {
            private readonly IBookPhotoRepository repository;
            private readonly ILoggedInUser loggedInUser;
            private readonly ILogger<Handler> logger;

            public Handler(IBookPhotoRepository repository, ILoggedInUser loggedInUser, ILogger<Handler> logger)
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

                Domain.Entities.BookPhoto bookPhoto = await repository.GetById(request.Id);

                if(bookPhoto == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.BookPhoto), request.Id);
                }

                File.Delete(bookPhoto.FilePath);

                await repository.Delete(bookPhoto);

                logger.LogInformation($"{nameof(Domain.Entities.BookPhoto)} with Id: {request.Id} deleted by {loggedInUser.UserId} at {DateTime.Now}");

                return default;
            }
        }
    }
}
