using BookShop.Core.Abstract.Repositories;
using FluentValidation;

namespace BookShop.Core.Mediatr.Category.Commands.Update
{
    internal class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryCommand>
    {
        internal UpdateCategoryRequestValidator(ICategoryRepository repository)
        {
            RuleFor(command => command)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null");

            RuleFor(command => command.Name)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null")
                .NotEmpty()
                    .WithMessage("{PropertyName} cannot be empty")
                .Must(repository.IsUniqueName)
                    .WithMessage("Category with specified name already exists: {PropertyName}")
                .Length(2, 100)
                    .WithMessage("{PropertyName} must contain from 2 to 100 characters");
        }
    }
}
