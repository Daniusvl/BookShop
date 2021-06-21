using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Product.Queries.Where
{
    public static class WhereProduct
    {
        public record Query(Func<Domain.Entities.Product, bool> Func) :IRequest<IList<ProductModel>>;

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
                if (request == null)
                {
                    throw new ValidationException("Query cannot be null");
                }

                if (request.Func == null)
                {
                    throw new ValidationException("Func cannot be null");
                }

                IList<Domain.Entities.Product> products = await repository.Where(request.Func);

                if(products == null || products.Count == 0)
                {
                    return new List<ProductModel>();
                }

                IList<ProductModel> productModels = mapper.Map<IList<Domain.Entities.Product>, IList<ProductModel>>(products);

                return productModels;
            }
        }
    }
}
