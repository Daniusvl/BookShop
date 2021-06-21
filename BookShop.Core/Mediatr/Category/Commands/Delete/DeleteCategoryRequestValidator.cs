using FluentValidation;

namespace BookShop.Core.Mediatr.Category.Commands.Delete
{
    internal class DeleteCategoryRequestValidator : AbstractValidator<DeleteCategoryCommand>
    {
        internal DeleteCategoryRequestValidator()
        {
            RuleFor(command => command)
                .NotNull()
                    .WithMessage("{propertyName} cannot be null");
        }
    }
}
