using FluentValidation;

namespace BookShop.Core.Mediatr.Author.Commands.Delete
{
    internal class DeleteAuthorRequestValidator : AbstractValidator<DeleteAuthorCommand>
    {
        internal DeleteAuthorRequestValidator()
        {
            RuleFor(command => command)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null");
        }
    }
}
