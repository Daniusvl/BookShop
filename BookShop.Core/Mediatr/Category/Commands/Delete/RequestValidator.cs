using FluentValidation;

namespace BookShop.Core.Mediatr.Category.Commands.Delete
{
    internal class RequestValidator : AbstractValidator<DeleteCategory.Command>
    {
        internal RequestValidator()
        {
            RuleFor(command => command)
                .NotNull()
                    .WithMessage("{propertyName} cannot be null");
        }
    }
}
