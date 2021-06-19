using BookShop.Core.Abstract.Repositories;
using FluentValidation;

namespace BookShop.Core.Mediatr.BookPhoto.Commands.Update
{
    internal class RequestValidator : AbstractValidator<UpdateBookPhoto.Command>
    {
        private readonly IBookPhotoRepository repository;

        internal RequestValidator(IBookPhotoRepository repository)
        {
            this.repository = repository;

            RuleFor(command => command)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null");

            RuleFor(command => command.BookPhoto)
                .NotNull()
                    .WithMessage("{PropertyName} cannot be null");

            RuleFor(command => command.BookPhoto.FilePath)
                .Must(repository.PhotoExists)
                    .WithMessage("Photo in this directory does not exist: {PropertyName}");
        }
    }
}
