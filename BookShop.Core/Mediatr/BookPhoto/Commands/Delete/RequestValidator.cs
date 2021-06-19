using FluentValidation;

namespace BookShop.Core.Mediatr.BookPhoto.Commands.Delete
{
    internal class RequestValidator : AbstractValidator<DeleteBookPhoto.Command>
    {
        internal RequestValidator()
        {
            RuleFor(command => command)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null");
        }
    }
}
