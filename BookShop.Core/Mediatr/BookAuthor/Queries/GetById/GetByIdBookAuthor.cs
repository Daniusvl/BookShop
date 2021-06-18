using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Configuration;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.BookAuthor.Queries.GetById
{
    public static class GetByIdBookAuthor
    {
        public record Query(int id) : IRequest<BookAuthorModel>;

        public class Handler : IRequestHandler<Query, BookAuthorModel>
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

            public async Task<BookAuthorModel> Handle(Query request, CancellationToken cancellationToken)
            {
                if (repository == null)
                {
                    throw new ServiceNullException(nameof(IProductRepository), nameof(Handler));
                }

                if (configuration == null)
                {
                    throw new ServiceNullException(nameof(IConfiguration), nameof(Handler));
                }

                if(mapper == null)
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

                Domain.Entities.BookAuthor entity = await repository.GetById(request.id);

                if(entity == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.BookAuthor), request.id);
                }

                BookAuthorModel model = mapper.Map<Domain.Entities.BookAuthor, BookAuthorModel>(entity);

                return model;
            }
        }
    }
}
