using FluentValidation;

namespace BookShop.Core.Mediatr.Photo.Commands.Delete
{
    internal class DeletePhotoRequestValidator : AbstractValidator<DeletePhotoCommand>
    {
        internal DeletePhotoRequestValidator()
        {
            RuleFor(command => command)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null");
        }
    }
}
