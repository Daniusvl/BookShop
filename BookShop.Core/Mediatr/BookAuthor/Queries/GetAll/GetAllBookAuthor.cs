using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.BookAuthor.Queries.GetAll
{
    public static class GetAllBookAuthor
    {
        public record Query : IRequest<IList<BookAuthorModel>>;

        public class Handler : IRequestHandler<Query, IList<BookAuthorModel>>
        {
            private readonly IBookAuthorRepository repository;
            private readonly IMapper mapper;

            public Handler(IBookAuthorRepository repository, IMapper mapper)
            {
                this.repository = repository;
                this.mapper = mapper;
            }

            public async Task<IList<BookAuthorModel>> Handle(Query request, CancellationToken cancellationToken)
            {
                IList<Domain.Entities.BookAuthor> bookAuthors = await repository.GetAll();

                if(bookAuthors == null || bookAuthors.Count == 0)
                {
                    return new List<BookAuthorModel>();
                }

                IList<BookAuthorModel> bookAuthorModels = mapper.Map<IList<Domain.Entities.BookAuthor>, IList<BookAuthorModel>>(bookAuthors);

                return bookAuthorModels;
            }
        }
    }
}
