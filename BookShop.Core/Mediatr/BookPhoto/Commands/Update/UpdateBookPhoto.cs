using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Configuration;
using BookShop.Core.Exceptions;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.BookPhoto.Commands.Update
{
    public static class UpdateBookPhoto
    {
        public record Command(Domain.Entities.BookPhoto BookPhoto) : IRequest;

        public class Handler : IRequestHandler<Command>
        {
            private readonly IBookPhotoRepository repository;
            private readonly ILogger<Handler> logger;

            public Handler(IBookPhotoRepository repository, ILogger<Handler> logger)
            {
                this.repository = repository;
                this.logger = logger;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if (repository == null)
                {
                    throw new ServiceNullException(nameof(IBookPhotoRepository), nameof(Handler));
                }

                RequestValidator validator = new(repository);
                ValidationResult result = await validator.ValidateAsync(request);

                if(result.Errors.Count > 0)
                {
                    throw new ValidationException(result);
                }

                Domain.Entities.BookPhoto bookPhoto = await repository.GetById(request.BookPhoto.Id);

                if(bookPhoto == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.BookPhoto), request.BookPhoto.Id);
                }

                if(bookPhoto.FilePath != request.BookPhoto.FilePath)
                {
                    result.Errors.Add(new ValidationFailure("FilePath"
                        , "You cannot change FilePath. If you want to update file it self, delete the old BookPhoto and ad new one"));
                    throw new ValidationException(result);
                }

                await repository.Update(request.BookPhoto);

                logger.LogInformation($"{nameof(Domain.Entities.BookPhoto)} with Id: {bookPhoto.Id} modified by {bookPhoto.LastModifiedBy} at {bookPhoto.DateLastModified}");


                return default;
            }
        }
    }
}
