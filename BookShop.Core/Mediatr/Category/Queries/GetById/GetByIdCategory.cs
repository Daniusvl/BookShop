using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Configuration;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Category.Queries.GetById
{
    public static class GetByIdCategory
    {
        public record Query(int Id) : IRequest<CategoryModel>;

        public class Handler : IRequestHandler<Query, CategoryModel>
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

            public async Task<CategoryModel> Handle(Query request, CancellationToken cancellationToken)
            {
                if (repository == null)
                {
                    throw new ServiceNullException(nameof(ICategoryRepository), nameof(Handler));
                }

                if (configuration == null)
                {
                    throw new ServiceNullException(nameof(IConfiguration), nameof(Handler));
                }

                if (mapper == null)
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

                Domain.Entities.Category category = await repository.GetById(request.Id);

                if(category == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.Category), request.Id);
                }

                CategoryModel model = mapper.Map<Domain.Entities.Category, CategoryModel>(category);

                return model;
            }
        }
    }
}
