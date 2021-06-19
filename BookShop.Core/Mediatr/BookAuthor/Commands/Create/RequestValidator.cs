using BookShop.Core.Abstract.Repositories;
using FluentValidation;

namespace BookShop.Core.Mediatr.BookAuthor.Commands.Create
{
    internal class RequestValidator : AbstractValidator<CreateBookAuthor.Command>
    {
        private readonly IBookAuthorRepository repository;

        internal RequestValidator(IBookAuthorRepository repository)
        {
            this.repository = repository;

            RuleFor(command => command)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null");

            RuleFor(command => command.Name)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null")
                .NotEmpty()
                    .WithMessage("{PropertyName} cannot be empty")
                .MaximumLength(200)
                    .WithMessage("{PropertyName} cannot be more than 200 characters")
                .Must(repository.IsUniqueName)
                    .WithMessage("There is already author with this name: {PropertyName}");
        }
    }
}
