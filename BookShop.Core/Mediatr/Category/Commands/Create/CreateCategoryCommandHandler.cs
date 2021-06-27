using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Category.Commands.Create
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryModel>
    {
        private readonly ICategoryRepository repository;
        private readonly IMapper mapper;
        private readonly ILogger<CreateCategoryCommandHandler> logger;

        public CreateCategoryCommandHandler(ICategoryRepository repository, 
            IMapper mapper, ILogger<CreateCategoryCommandHandler> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<CategoryModel> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            CreateCategoryRequestValidator validator = new(repository);
            ValidationResult result = await validator.ValidateAsync(request);

            if (result.Errors.Count > 0)
            {
                throw new ValidationException(result);
            }

            Domain.Entities.Category category = new()
            {
                Name = request?.Name ?? string.Empty
            };

            await repository.BaseRepository.Create(category);

            logger.LogInformation($"{nameof(Domain.Entities.Category)} with Id: {category.Id} created by {category.CreatedBy} at {category.DateCreated}");

            return mapper.Map<Domain.Entities.Category, CategoryModel>(category);
        }
    }
}
