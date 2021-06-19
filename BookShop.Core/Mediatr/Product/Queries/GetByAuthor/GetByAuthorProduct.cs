using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Product.Queries.GetByAuthor
{
    public static class GetByAuthorProduct
    {
        public record Query(Domain.Entities.BookAuthor Author) : IRequest<IList<ProductModel>>;

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
                if (repository == null)
                {
                    throw new ServiceNullException(nameof(IProductRepository), nameof(Handler));
                }

                if (mapper == null)
                {
                    throw new ServiceNullException(nameof(IMapper), nameof(Handler));
                }

                if (request == null)
                {
                    throw new ValidationException("Query cannot be null");
                }

                IList<Domain.Entities.Product> products = repository.GetByAuthor(request.Author);

                if (products == null || products.Count == 0)
                {
                    return new List<ProductModel>();
                }

                IList<ProductModel> productModels = mapper.Map<IList<Domain.Entities.Product>, IList<ProductModel>>(products);

                return productModels;
            }
        }
    }
}
