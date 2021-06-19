using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.BookAuthor.Queries.Where
{
    public static class WhereBookAuthor
    {
        public record Query(Func<Domain.Entities.BookAuthor, bool> Func) : IRequest<IList<BookAuthorModel>>;

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
                if (repository == null)
                {
                    throw new ServiceNullException(nameof(IBookAuthorRepository), nameof(Handler));
                }

                if (mapper == null)
                {
                    throw new ServiceNullException(nameof(IMapper), nameof(Handler));
                }

                if (request == null)
                {
                    throw new ValidationException("Query cannot be null");
                }

                if(request.Func == null)
                {
                    throw new ValidationException("Func cannot be null");
                }

                IList<Domain.Entities.BookAuthor> bookAuthors = await repository.Where(request.Func);

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
