using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Book.Queries
{
    public class GetAllBookQuery : IRequest<IList<BookModel>> { }

    public class GetAllBookQueryHandler : IRequestHandler<GetAllBookQuery, IList<BookModel>>
    {
        private readonly IBookRepository repository;
        private readonly IMapper mapper;

        public GetAllBookQueryHandler(IBookRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IList<BookModel>> Handle(GetAllBookQuery request, CancellationToken cancellationToken)
        {
            IList<Domain.Entities.Book> products = await repository.GetAll();

            if (products == null || products.Count == 0)
            {
                return new List<BookModel>();
            }

            IList<BookModel> models = mapper.Map<IList<Domain.Entities.Book>, IList<BookModel>>(products);

            return models;
        }
    }
}
