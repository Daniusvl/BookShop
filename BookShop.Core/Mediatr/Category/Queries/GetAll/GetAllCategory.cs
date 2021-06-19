using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Configuration;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
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
            private readonly IConfiguration configuration;
            private readonly IMapper mapper;

            public Handler(ICategoryRepository repository, IConfiguration configuration, IMapper mapper)
            {
                this.repository = repository;
                this.configuration = configuration;
                this.mapper = mapper;
            }

            public async Task<IList<CategoryModel>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (repository == null)
                {
                    throw new ServiceNullException(nameof(ICategoryRepository), nameof(Handler));
                }

                if (configuration == null)
                {
                    throw new ServiceNullException(nameof(IConfiguration), nameof(Handler));
                }

                if(mapper == null)
                {
                    throw new ServiceNullException(nameof(IMapper), nameof(Handler));
                }

                if (configuration.IsDevelopment())
                {
                    if (request == null)
                    {
                        throw new ArgumentNullException(nameof(request));
                    }
                }

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
