using FluentValidation;

namespace BookShop.Core.Mediatr.Book.Commands.Delete
{
    internal class DeleteBookRequestValidator : AbstractValidator<DeleteBookCommand>
    {
        internal DeleteBookRequestValidator()
        {
            RuleFor(command => command)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null");
        }
    }
}
