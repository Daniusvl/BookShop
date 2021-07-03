using BookShop.Core.Abstract.Repositories;
using FluentValidation;

namespace BookShop.Core.Mediatr.Photo.Commands.Create
{
    internal class CreatePhotoRequestValidator : AbstractValidator<CreatePhotoCommand>
    {
        internal CreatePhotoRequestValidator()
        {
            RuleFor(command => command)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null");
        }
    }
}
