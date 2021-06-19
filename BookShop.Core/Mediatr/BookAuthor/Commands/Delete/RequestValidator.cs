using FluentValidation;

namespace BookShop.Core.Mediatr.BookAuthor.Commands.Delete
{
    internal class RequestValidator : AbstractValidator<DeleteBookAuthor.Command>
    {
        internal RequestValidator()
        {
            RuleFor(command => command)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null");
        }
    }
}
