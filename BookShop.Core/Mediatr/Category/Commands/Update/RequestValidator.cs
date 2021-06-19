using BookShop.Core.Abstract.Repositories;
using FluentValidation;

namespace BookShop.Core.Mediatr.Category.Commands.Update
{
    internal class RequestValidator : AbstractValidator<UpdateCategory.Command>
    {
        private readonly ICategoryRepository repository;

        internal RequestValidator(ICategoryRepository repository)
        {
            this.repository = repository;

            RuleFor(command => command)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null");

            RuleFor(command => command.Category)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null");

            RuleFor(command => command.Category.Name)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null")
                .NotEmpty()
                    .WithMessage("{PropertyName} cannot be empty")
                .Must(repository.IsUniqueName)
                    .WithMessage("Category with specified name already exists: {PropertyName}");
        }
    }
}
