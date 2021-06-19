using BookShop.Core.Abstract.Repositories;
using FluentValidation;

namespace BookShop.Core.Mediatr.BookAuthor.Commands.Update
{
    internal class RequestValidator : AbstractValidator<UpdateBookAuthor.Command>
    {
        private readonly IBookAuthorRepository repository;

        internal RequestValidator(IBookAuthorRepository repository)
        {
            this.repository = repository;

            RuleFor(command => command)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null");

            RuleFor(command => command.BookAuthor)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null");

            RuleFor(ent => ent.BookAuthor.Name)
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
