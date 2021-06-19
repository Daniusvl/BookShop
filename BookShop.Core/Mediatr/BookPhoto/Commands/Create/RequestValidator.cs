using BookShop.Core.Abstract.Repositories;
using FluentValidation;

namespace BookShop.Core.Mediatr.BookPhoto.Commands.Create
{
    public class RequestValidator : AbstractValidator<CreateBookPhoto.Command>
    {
        private readonly IProductRepository repository;

        public RequestValidator(IProductRepository repository)
        {
            this.repository = repository;
            
            RuleFor(command => command)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null");

            RuleFor(command => command.ProductName)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null")
                .NotEmpty()
                    .WithMessage("{PropertyName} cannot be empty")
                .Must(repository.ContainsWithName)
                    .WithMessage("Where is no product with name: {PropertyName}");

            RuleFor(command => command.FileBytes)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null")
                .NotEmpty()
                    .WithMessage("{PropertyName} cannot be empty");
        }
    }
}
