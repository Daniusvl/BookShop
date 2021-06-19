using BookShop.Core.Abstract.Repositories;
using FluentValidation;

namespace BookShop.Core.Mediatr.BookAuthor.Commands.Create
{
    internal class RequestValidator : AbstractValidator<string>
    {
        private readonly IBookAuthorRepository repository;

        public RequestValidator(IBookAuthorRepository repository)
        {
            this.repository = repository;

            RuleFor(s => s)
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
