using BookShop.Core.Abstract.Repositories;
using FluentValidation;

namespace BookShop.Core.Mediatr.Photo.Commands.Create
{
    internal class CreatePhotoRequestValidator : AbstractValidator<CreatePhotoCommand>
    {
        internal CreatePhotoRequestValidator(IBookRepository repository)
        {
            RuleFor(command => command)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null");

            RuleFor(command => command.ProductName)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null")
                .NotEmpty()
                    .WithMessage("{PropertyName} cannot be empty")
                .MustAsync(async (name, token) => await repository.ContainsWithName(name))
                    .WithMessage("Where is no product with name: {PropertyName}");
        }
    }
}
