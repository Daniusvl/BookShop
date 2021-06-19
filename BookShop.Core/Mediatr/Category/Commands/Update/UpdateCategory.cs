using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Configuration;
using BookShop.Core.Exceptions;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Category.Commands.Update
{
    public static class UpdateCategory
    {
        public record Command(Domain.Entities.Category Category) : IRequest;

        public class Handler : IRequestHandler<Command>
        {
            private readonly ICategoryRepository repository;

            public Handler(ICategoryRepository repository)
            {
                this.repository = repository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if (repository == null)
                {
                    throw new ServiceNullException(nameof(ICategoryRepository), nameof(Handler));
                }

                RequestValidator validator = new(repository);
                ValidationResult result = await validator.ValidateAsync(request);

                if(result.Errors.Count > 0)
                {
                    throw new ValidationException(result);
                }

                Domain.Entities.Category category = await repository.GetById(request.Category.Id);

                if(category == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.Category), request.Category.Id);
                }

                await repository.Update(category);

                return default;
            }
        }
    }
}
