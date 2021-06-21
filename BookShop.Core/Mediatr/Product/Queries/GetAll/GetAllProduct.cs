using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using MediatR;
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
            private readonly IMapper mapper;

            public Handler(IProductRepository repository, IMapper mapper)
            {
                this.repository = repository;
                this.mapper = mapper;
            }

            public async Task<IList<ProductModel>> Handle(Query request, CancellationToken cancellationToken)
            {
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
