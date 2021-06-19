using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using FluentValidation.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.BookAuthor.Commands.Create
{
    public static class CreateBookAuthor
    {
        public record Command(string Name) : IRequest;

        public class Handler : IRequestHandler<Command>
        {
            private readonly IBookAuthorRepository repository;;

            public Handler(IBookAuthorRepository repository)
            {
                this.repository = repository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if(repository == null)
                {
                    throw new ServiceNullException(nameof(IProductRepository), nameof(Handler));
                }

                RequestValidator validator = new (repository);
                ValidationResult result = await validator.ValidateAsync(request);

                if (result.Errors.Count > 0)
                {
                    throw new ValidationException(result);
                }

                // TODO: Add validation and logging.

                Domain.Entities.BookAuthor bookAuthor = new()
                {
                    Name = request.Name
                };

                await repository.Create(bookAuthor);

                return default;
            }
        }
    }
}
