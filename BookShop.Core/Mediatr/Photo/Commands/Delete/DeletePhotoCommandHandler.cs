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

namespace BookShop.Core.Mediatr.Photo.Commands.Delete
{
    public class DeletePhotoCommandHandler : IRequestHandler<DeletePhotoCommand>
    {
        private readonly IPhotoRepository repository;
        private readonly ILoggedInUser loggedInUser;
        private readonly ILogger<DeletePhotoCommandHandler> logger;

        public DeletePhotoCommandHandler(IPhotoRepository repository, ILoggedInUser loggedInUser, ILogger<DeletePhotoCommandHandler> logger)
        {
            this.repository = repository;
            this.loggedInUser = loggedInUser;
            this.logger = logger;
        }

        public async Task<Unit> Handle(DeletePhotoCommand request, CancellationToken cancellationToken)
        {
            DeletePhotoRequestValidator validator = new();
            ValidationResult result = await validator.ValidateAsync(request);

            if (result.Errors.Count > 0)
            {
                throw new ValidationException(result);
            }

            Domain.Entities.Photo photo = await repository.GetById(request.Id);

            if (photo == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Photo), request.Id);
            }

            File.Delete(photo.FilePath);

            await repository.Delete(photo);

            logger.LogInformation($"{nameof(Domain.Entities.Photo)} with Id: {request.Id} deleted by {loggedInUser.UserId} at {DateTime.Now}");

            return default;
        }
    }
}
