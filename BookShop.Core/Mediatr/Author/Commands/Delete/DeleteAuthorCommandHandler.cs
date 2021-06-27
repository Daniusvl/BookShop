using BookShop.Core.Abstract;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Author.Commands.Delete
{
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand>
    {
        private readonly IAuthorRepository repository;
        private readonly ILoggedInUser loggedInUser;
        private readonly ILogger<DeleteAuthorCommandHandler> logger;

        public DeleteAuthorCommandHandler(IAuthorRepository repository, ILoggedInUser loggedInUser, ILogger<DeleteAuthorCommandHandler> logger)
        {
            this.repository = repository;
            this.loggedInUser = loggedInUser;
            this.logger = logger;
        }

        public async Task<Unit> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            DeleteAuthorRequestValidator validatiors = new();
            ValidationResult result = await validatiors.ValidateAsync(request);

            if (result.Errors.Count > 0)
            {
                throw new ValidationException(result);
            }

            Domain.Entities.Author author = await repository.BaseRepository.GetById(request.Id);

            if (author == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Author), request.Id);
            }

            await repository.BaseRepository.Delete(author);

            logger.LogInformation($"{nameof(Domain.Entities.Author)} with Id: {request.Id} deleted by {loggedInUser.UserId} at {DateTime.Now}");

            return default;
        }
    }
}
