using BookShop.Core.Abstract.Repositories;
using FluentValidation;

namespace BookShop.Core.Mediatr.Category.Commands.Create
{
    internal class RequestValidator : AbstractValidator<CreateCategory.Command>
    {
        private readonly ICategoryRepository repository;

        internal RequestValidator(ICategoryRepository repository)
        {
            this.repository = repository;

            RuleFor(command => command)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null");

            RuleFor(command => command.Name)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null")
                .NotEmpty()
                    .WithMessage("{PropertyName} cannot be empty")
                .Must(repository.IsUniqueName)
                    .WithMessage("Category with specified name already exists: {PropertyName}");
                
        }
    }
}
