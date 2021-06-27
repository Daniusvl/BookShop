using BookShop.Core.Abstract.Repositories;
using FluentValidation;

namespace BookShop.Core.Mediatr.Author.Commands.Create
{
    internal class CreateAuthorRequestValidator : AbstractValidator<CreateAuthorCommand>
    {
        internal CreateAuthorRequestValidator(IAuthorRepository repository)
        {
            RuleFor(command => command)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null");

            RuleFor(command => command.Name)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null")
                .NotEmpty()
                    .WithMessage("{PropertyName} cannot be empty")
                .Length(5, 200)
                    .WithMessage("{PropertyName} must contain from 5 to 200 characters")
                .MustAsync(async (name, token) => await repository.IsUniqueName(name))
                    .WithMessage("There is already author with this name: {PropertyName}");
        }
    }
}
