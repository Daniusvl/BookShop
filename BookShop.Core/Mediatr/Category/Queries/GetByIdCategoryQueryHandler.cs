using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Category.Queries
{
    public class GetByIdCategoryQuery : IRequest<CategoryModel>
    {
        public int Id { get; set; }

        public GetByIdCategoryQuery(int id)
        {
            Id = id;
        }
    }

    public class GetByIdCategoryQueryHandler : IRequestHandler<GetByIdCategoryQuery, CategoryModel>
    {
        private readonly ICategoryRepository repository;
        private readonly IMapper mapper;

        public GetByIdCategoryQueryHandler(ICategoryRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<CategoryModel> Handle(GetByIdCategoryQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ValidationException("Query cannot be null");
            }

            Domain.Entities.Category category = await repository.GetById(request.Id);

            if (category == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Category), request.Id);
            }

            CategoryModel model = mapper.Map<Domain.Entities.Category, CategoryModel>(category);

            return model;
        }
    }
}
