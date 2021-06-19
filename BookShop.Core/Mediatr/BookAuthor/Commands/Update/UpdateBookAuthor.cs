using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using FluentValidation.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.BookAuthor.Commands.Update
{
    public static class UpdateBookAuthor
    {
        public record Command(Domain.Entities.BookAuthor BookAuthor) : IRequest;

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
                    throw new ServiceNullException(nameof(IBookAuthorRepository), nameof(Handler));
                }

                RequestValidator validator = new(repository);
                ValidationResult result = await validator.ValidateAsync(request);
                
                if(result.Errors.Count > 0)
                {
                    throw new ValidationException(result);
                }

                Domain.Entities.BookAuthor bookAuthor = await repository.GetById(request.BookAuthor.Id);

                if(bookAuthor == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.BookAuthor), request.BookAuthor.Id);
                }

                await repository.Update(request?.BookAuthor);

                return default;
            }
        }
    }
}
