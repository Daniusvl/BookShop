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
        public record Query(int AuthorId) : IRequest<IList<ProductModel>>;

        public class Handler : IRequestHandler<Query, IList<ProductModel>>
        {
            private readonly IProductRepository repository;
            private readonly IBookAuthorRepository bookAuthorRepository;
            private readonly IMapper mapper;

            public Handler(IProductRepository repository, IBookAuthorRepository bookAuthorRepository, IMapper mapper)
            {
                this.repository = repository;
                this.bookAuthorRepository = bookAuthorRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ProductModel>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (request == null)
                {
                    throw new ValidationException("Query cannot be null");
                }

                Domain.Entities.BookAuthor author = await bookAuthorRepository.GetById(request.AuthorId);

                if(author == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.BookAuthor), author.Id);
                }

                IList<Domain.Entities.Product> products = repository.GetByAuthor(author);

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
