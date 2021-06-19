using FluentValidation;

namespace BookShop.Core.Mediatr.Product.Commands.Delete
{
    internal class RequestValidator : AbstractValidator<DeleteProduct.Command>
    {
        internal RequestValidator()
        {
            RuleFor(command => command)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null");
        }
    }
}
