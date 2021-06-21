using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using MediatR;
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
            private readonly IMapper mapper;

            public Handler(IBookAuthorRepository repository, IMapper mapper)
            {
                this.repository = repository;
                this.mapper = mapper;
            }

            public async Task<BookAuthorModel> Handle(Query request, CancellationToken cancellationToken)
            {
                if(request == null)
                {
                    throw new ValidationException("Query cannot be null");
                }

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
