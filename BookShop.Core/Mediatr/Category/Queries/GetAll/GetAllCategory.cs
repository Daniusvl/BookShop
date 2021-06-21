using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Category.Queries.GetAll
{
    public static class GetAllCategory
    {
        public record Query() : IRequest<IList<CategoryModel>>;

        public class Handler : IRequestHandler<Query, IList<CategoryModel>>
        {
            private readonly ICategoryRepository repository;
            private readonly IMapper mapper;

            public Handler(ICategoryRepository repository, IMapper mapper)
            {
                this.repository = repository;
                this.mapper = mapper;
            }

            public async Task<IList<CategoryModel>> Handle(Query request, CancellationToken cancellationToken)
            {
                IList<Domain.Entities.Category> categories = await repository.GetAll();

                if(categories == null || categories.Count == 0)
                {
                    return new List<CategoryModel>();
                }

                IList<CategoryModel> categoryModels = mapper.Map<IList<Domain.Entities.Category>, IList<CategoryModel>>(categories);

                return categoryModels;
            }
        }
    }
}
