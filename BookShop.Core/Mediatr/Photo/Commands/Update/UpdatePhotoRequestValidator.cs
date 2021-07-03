using FluentValidation;

namespace BookShop.Core.Mediatr.Photo.Commands.Update
{
    public class UpdatePhotoRequestValidator : AbstractValidator<UpdatePhotoCommand>
    {
        public UpdatePhotoRequestValidator()
        {
            RuleFor(command => command)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null");
        }
    }
}
