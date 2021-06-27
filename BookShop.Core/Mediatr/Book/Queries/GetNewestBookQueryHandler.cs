using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Book.Queries
{
    public class GetNewestBookQuery : IRequest<IList<BookModel>> { }

    public class GetNewestBookQueryHandler : IRequestHandler<GetNewestBookQuery, IList<BookModel>>
    {
        private readonly IBookRepository repository;
        private readonly IMapper mapper;

        public GetNewestBookQueryHandler(IBookRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IList<BookModel>> Handle(GetNewestBookQuery request, CancellationToken cancellationToken)
        {
            IList<Domain.Entities.Book> books = await repository.GetNewest(20);

            if (books == null || books.Count == 0)
            {
                return new List<BookModel>();
            }

            IList<BookModel> models = mapper.Map<IList<Domain.Entities.Book>, IList<BookModel>>(books);

            return models;
        }
    }
}
