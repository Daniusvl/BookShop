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

namespace BookShop.Core.Mediatr.Product.Queries.GetAll
{
    public static class GetAllProduct
    {
        public record Query() : IRequest<IList<ProductModel>>;

        public class Handler : IRequestHandler<Query, IList<ProductModel>>
        {
            private readonly IProductRepository repository;
            private readonly IConfiguration configuration;
            private readonly IMapper mapper;

            public Handler(IProductRepository repository, IConfiguration configuration, IMapper mapper)
            {
                this.repository = repository;
                this.configuration = configuration;
                this.mapper = mapper;
            }

            public async Task<IList<ProductModel>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (repository == null)
                {
                    throw new ServiceNullException(nameof(IProductRepository), nameof(Handler));
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

                // TODO: Add validation and logging.

                IList<Domain.Entities.Product> products = await repository.GetAll();

                if(products == null || products.Count == 0)
                {
                    return new List<ProductModel>();
                }

                IList<ProductModel> models = mapper.Map<IList<Domain.Entities.Product>, IList<ProductModel>>(products);

                return models;
            }
        }
    }
}
