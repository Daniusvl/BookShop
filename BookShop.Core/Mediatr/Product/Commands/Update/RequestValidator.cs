using BookShop.Core.Abstract.Repositories;
using FluentValidation;

namespace BookShop.Core.Mediatr.Product.Commands.Update
{
    internal class RequestValidator : AbstractValidator<UpdateProduct.Command>
    {
        private readonly IProductRepository repository;

        internal RequestValidator(IProductRepository repository)
        {
            this.repository = repository;

            RuleFor(command => command)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null");

            RuleFor(command => command.Product)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null");

            RuleFor(command => command.Product.Name)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null")
                .NotEmpty()
                    .WithMessage("{PropertyName} cannot be empty")
                .Must(repository.IsUniqueName)
                    .WithMessage("Product with specified name already exists: {PropertyName}")
                .Length(3, 150)
                    .WithMessage("{PropertyName} must contain from 3 to 150 characters");

            RuleFor(command => command.Product.Description)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null")
                .NotEmpty()
                    .WithMessage("{PropertyName} cannot be empty")
                .Length(10, 1000)
                    .WithMessage("Description must contain from 10 to 1000 characters");

            RuleFor(command => command.Product.Price)
                .GreaterThan(0)
                    .WithMessage("{PropertyName} must be more than 0");

            RuleFor(command => command.Product.Author)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null");

            RuleFor(command => command.Product.Category)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null");
        }
    }
}
