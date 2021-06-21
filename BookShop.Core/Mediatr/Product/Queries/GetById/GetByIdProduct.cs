using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Product.Queries.GetById
{
    public static class GetByIdProduct
    {
        public record Query(int Id) : IRequest<ProductModel>;

        public class Handler : IRequestHandler<Query, ProductModel>
        {
            private readonly IProductRepository repository;
            private readonly IMapper mapper;

            public Handler(IProductRepository repository, IMapper mapper)
            {
                this.repository = repository;
                this.mapper = mapper;
            }

            public async Task<ProductModel> Handle(Query request, CancellationToken cancellationToken)
            {
                if (request == null)
                {
                    throw new ValidationException("Query cannot be null");
                }

                Domain.Entities.Product product = await repository.GetById(request.Id);

                if(product == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.Product), request.Id);
                }

                ProductModel model = mapper.Map<Domain.Entities.Product, ProductModel>(product);

                return model;
            }
        }
    }
}
