using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Book.Queries
{
    public class GetByNameBookQuery : IRequest<BookModel>
    {
        public string Name { get; set; }

        public GetByNameBookQuery(string name)
        {
            Name = name;
        }
    }

    public class GetByNameBookQueryHandler : IRequestHandler<GetByNameBookQuery, BookModel>
    {
        private readonly IBookRepository repository;
        private readonly IMapper mapper;

        public GetByNameBookQueryHandler(IBookRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<BookModel> Handle(GetByNameBookQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ValidationException("Query cannot be null");
            }

            Domain.Entities.Book book = repository.GetByName(request.Name);

            if (book == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Book), -1337);
            }

            BookModel model = mapper.Map<Domain.Entities.Book, BookModel>(book);

            return model;
        }
    }
}
