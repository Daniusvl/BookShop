using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Author.Commands.Update
{
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, AuthorModel>
    {
        private readonly IAuthorRepository repository;
        private readonly IMapper mapper;
        private readonly ILogger<UpdateAuthorCommandHandler> logger;

        public UpdateAuthorCommandHandler(IAuthorRepository repository, IMapper mapper, ILogger<UpdateAuthorCommandHandler> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<AuthorModel> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            UpdateAuthorRequestValidator validator = new(repository);
            ValidationResult result = await validator.ValidateAsync(request);

            if (result.Errors.Count > 0)
            {
                throw new ValidationException(result);
            }

            Domain.Entities.Author author = await repository.GetById(request.Id);

            if (author == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Author), request.Id);
            }

            author.Name = request.Name;

            await repository.Update(author);

            logger.LogInformation($"{nameof(Domain.Entities.Author)} with Id: {author.Id} updated by {author.LastModifiedBy} at {author.DateLastModified}");


            return mapper.Map<Domain.Entities.Author, AuthorModel>(author);
        }
    }
}
