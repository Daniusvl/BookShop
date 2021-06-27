using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Author.Commands.Create
{
    public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, AuthorModel>
    {
        private readonly IAuthorRepository repository;
        private readonly IMapper mapper;
        private readonly ILogger<CreateAuthorCommandHandler> logger;

        public CreateAuthorCommandHandler(IAuthorRepository repository, IMapper mapper, ILogger<CreateAuthorCommandHandler> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<AuthorModel> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            CreateAuthorRequestValidator validator = new(repository);
            ValidationResult result = await validator.ValidateAsync(request);

            if (result.Errors.Count > 0)
            {
                throw new ValidationException(result);
            }

            Domain.Entities.Author author = new()
            {
                Name = request.Name
            };

            await repository.BaseRepository.Create(author);

            logger.LogInformation($"{nameof(Domain.Entities.Author)} with Id: {author.Id} created by {author.CreatedBy} at {author.DateCreated}");

            return mapper.Map<Domain.Entities.Author, AuthorModel>(author);
        }
    }
}
