using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Book.Queries
{
    public class GetByPriceBookQuery : IRequest<IList<BookModel>>
    {
        public decimal Min { get; set; }
        public decimal Max { get; set; }

        public GetByPriceBookQuery(decimal min, decimal max)
        {
            Min = min;
            Max = max;
        }
    }

    public class GetByPriceBookQueryHandler : IRequestHandler<GetByPriceBookQuery, IList<BookModel>>
    {
        private readonly IBookRepository repository;
        private readonly IMapper mapper;

        public GetByPriceBookQueryHandler(IBookRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IList<BookModel>> Handle(GetByPriceBookQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ValidationException("Query cannot be null");
            }

            IList<Domain.Entities.Book> books = await repository.GetByPrice(request.Min, request.Max);

            if (books == null || books.Count == 0)
            {
                return new List<BookModel>();
            }

            IList<BookModel> bookModels = mapper.Map<IList<Domain.Entities.Book>, IList<BookModel>>(books);

            return bookModels;
        }
    }
}
