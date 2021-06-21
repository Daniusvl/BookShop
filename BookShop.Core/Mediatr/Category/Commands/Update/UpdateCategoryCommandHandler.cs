using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Category.Commands.Update
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryModel>
    {
        private readonly ICategoryRepository repository;
        private readonly IMapper mapper;
        private readonly ILogger<UpdateCategoryCommandHandler> logger;

        public UpdateCategoryCommandHandler(ICategoryRepository repository,
            IMapper mapper, ILogger<UpdateCategoryCommandHandler> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<CategoryModel> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            UpdateCategoryRequestValidator validator = new(repository);
            ValidationResult result = await validator.ValidateAsync(request);

            if (result.Errors.Count > 0)
            {
                throw new ValidationException(result);
            }

            Domain.Entities.Category category = await repository.GetById(request.Id);

            if (category == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Category), request.Id);
            }

            category.Name = request.Name;

            await repository.Update(category);

            logger.LogInformation($"{nameof(Domain.Entities.Category)} with Id: {category.Id} modified by {category.LastModifiedBy} at {category.DateLastModified}");

            return mapper.Map<Domain.Entities.Category, CategoryModel>(category);
        }
    }
}
