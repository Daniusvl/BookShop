using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Category.Queries.Where
{
    public static class WhereCategory
    {
        public record Query(Func<Domain.Entities.Category, bool> Func) : IRequest<IList<CategoryModel>>;

        public class Handler : IRequestHandler<Query, IList<CategoryModel>>
        {
            private readonly ICategoryRepository repository;
            private readonly IMapper mapper;

            public Handler(ICategoryRepository repository,, IMapper mapper)
            {
                this.repository = repository;
                this.mapper = mapper;
            }

            public async Task<IList<CategoryModel>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (repository == null)
                {
                    throw new ServiceNullException(nameof(ICategoryRepository), nameof(Handler));
                }

                if (mapper == null)
                {
                    throw new ServiceNullException(nameof(IMapper), nameof(Handler));
                }

                if (request == null)
                {
                    throw new ValidationException("Query cannot be null");
                }

                if (request.Func == null)
                {
                    throw new ValidationException("Func cannot be null");
                }

                IList<Domain.Entities.Category> categories = await repository.Where(request.Func);

                if (categories == null || categories.Count == 0)
                {
                    return new List<CategoryModel>();
                }

                IList<CategoryModel> categoryModels = mapper.Map<IList<Domain.Entities.Category>, IList<CategoryModel>>(categories);

                return categoryModels;
            }
        }
    }
}
