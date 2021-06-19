using FluentValidation;

namespace BookShop.Core.Mediatr.BookPhoto.Commands.Delete
{
    public class RequestValidator : AbstractValidator<DeleteBookPhoto.Command>
    {
        public RequestValidator()
        {
            RuleFor(command => command)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null");
        }
    }
}
