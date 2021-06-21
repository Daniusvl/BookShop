using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Product.Queries.GetByCategory
{
    public static class GetByAuthorCategory
    {
        public record Query(int CategoryId) : IRequest<IList<ProductModel>>;

        public class Handler : IRequestHandler<Query, IList<ProductModel>>
        {
            private readonly IProductRepository repository;
            private readonly ICategoryRepository categoryRepository;
            private readonly IMapper mapper;

            public Handler(IProductRepository repository, ICategoryRepository categoryRepository, IMapper mapper)
            {
                this.repository = repository;
                this.categoryRepository = categoryRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ProductModel>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (request == null)
                {
                    throw new ValidationException("Query cannot be null");
                }

                Domain.Entities.Category category = await categoryRepository.GetById(request.CategoryId);

                if(category == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.Category), request.CategoryId);
                }

                IList<Domain.Entities.Product> products = repository.GetByCategory(category);

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
