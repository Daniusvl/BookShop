using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Book.Queries
{
    public class GetByIdBookQuery : IRequest<BookModel>
    {
        public int Id { get; set; }

        public GetByIdBookQuery(int id)
        {
            Id = id;
        }
    }

    public class GetByIdBookQueryHandler : IRequestHandler<GetByIdBookQuery, BookModel>
    {
        private readonly IBookRepository repository;
        private readonly IMapper mapper;

        public GetByIdBookQueryHandler(IBookRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<BookModel> Handle(GetByIdBookQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ValidationException("Query cannot be null");
            }

            Domain.Entities.Book book = await repository.GetById(request.Id);

            if (book == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Book), request.Id);
            }

            BookModel model = mapper.Map<Domain.Entities.Book, BookModel>(book);

            return model;
        }
    }
}
