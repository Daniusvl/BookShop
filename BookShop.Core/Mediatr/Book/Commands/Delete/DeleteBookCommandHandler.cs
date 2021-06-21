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

namespace BookShop.Core.Mediatr.Book.Commands.Delete
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand>
    {
        private readonly IBookRepository repository;
        private readonly ILoggedInUser loggedInUser;
        private readonly ILogger<DeleteBookCommandHandler> logger;

        public DeleteBookCommandHandler(IBookRepository repository, ILoggedInUser loggedInUser, ILogger<DeleteBookCommandHandler> logger)
        {
            this.repository = repository;
            this.loggedInUser = loggedInUser;
            this.logger = logger;
        }

        public async Task<Unit> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            DeleteBookRequestValidator validator = new();
            ValidationResult result = await validator.ValidateAsync(request);

            if (result.Errors.Count > 0)
            {
                throw new ValidationException(result);
            }

            Domain.Entities.Book product = await repository.GetById(request.Id);

            if (product == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Book), request.Id);
            }

            File.Delete(product.FilePath);

            await repository.Delete(product);

            logger.LogInformation($"{nameof(Domain.Entities.Book)} with Id: {request.Id} deleted by {loggedInUser.UserId} at {DateTime.Now}");

            return default;
        }
    }
}
