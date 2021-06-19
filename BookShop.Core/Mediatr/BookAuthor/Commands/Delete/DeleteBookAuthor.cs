using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using FluentValidation.Results;
using MediatR;
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

            public Handler(IBookAuthorRepository repository)
            {
                this.repository = repository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if (repository == null)
                {
                    throw new ServiceNullException(nameof(IProductRepository), nameof(Handler));
                }

                RequestValidator validatiors = new();
                ValidationResult result = await validatiors.ValidateAsync(request);

                if(result.Errors.Count > 0)
                {
                    throw new ValidationException(result);
                }

                // TODO: Add validation and logging.

                Domain.Entities.BookAuthor bookAuthor = await repository.GetById(request.Id);

                if(bookAuthor == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.BookAuthor), request.Id);
                }

                await repository.Delete(bookAuthor);

                return default;
            }
        }
    }
}
