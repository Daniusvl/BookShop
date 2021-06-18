using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Configuration;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
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
            private readonly IConfiguration configuration;
            private readonly IMapper mapper;

            public Handler(IBookAuthorRepository repository, IConfiguration configuration, IMapper mapper)
            {
                this.repository = repository;
                this.configuration = configuration;
                this.mapper = mapper;
            }

            public async Task<IList<BookAuthorModel>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (repository == null)
                {
                    throw new ServiceNullException(nameof(IProductRepository), nameof(Handler));
                }

                if (configuration == null)
                {
                    throw new ServiceNullException(nameof(IConfiguration), nameof(Handler));
                }

                if (mapper == null)
                {
                    throw new ServiceNullException(nameof(IMapper), nameof(Handler));
                }

                if (configuration.IsDevelopment())
                {
                    if (request == null)
                    {
                        throw new ArgumentNullException(nameof(request));
                    }
                }

                // TODO: Add validation and logging.

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
