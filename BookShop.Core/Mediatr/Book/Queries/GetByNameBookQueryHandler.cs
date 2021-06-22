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
    public class GetByNameBookQuery : IRequest<IList<BookModel>>
    {
        public string Name { get; set; }

        public GetByNameBookQuery(string name)
        {
            Name = name;
        }
    }

    public class GetByNameBookQueryHandler : IRequestHandler<GetByNameBookQuery, IList<BookModel>>
    {
        private readonly IBookRepository repository;
        private readonly IMapper mapper;

        public GetByNameBookQueryHandler(IBookRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IList<BookModel>> Handle(GetByNameBookQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ValidationException("Query cannot be null");
            }

            IList<Domain.Entities.Book> books = repository.GetByName(request.Name);

            if (books == null || books.Count == 0)
            {
                return new List<BookModel>();
            }

            IList<BookModel> bookModels = mapper.Map<IList<Domain.Entities.Book>, IList<BookModel>>(books);

            return bookModels;
        }
    }
}
