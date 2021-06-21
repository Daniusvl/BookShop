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

            RuleFor(command => command.Name)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null")
                .NotEmpty()
                    .WithMessage("{PropertyName} cannot be empty")
                .Length(5, 200)
                    .WithMessage("{PropertyName} must contain from 5 to 200 characters")
                .Must(repository.IsUniqueName)
                    .WithMessage("There is already author with this name: {PropertyName}");
        }
    }
}
