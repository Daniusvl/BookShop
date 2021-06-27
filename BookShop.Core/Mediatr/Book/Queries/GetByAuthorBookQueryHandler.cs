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
    public class GetByAuthorBookQuery : IRequest<IList<BookModel>>
    {
        public int AuthorId { get; set; }

        public GetByAuthorBookQuery(int authorId)
        {
            AuthorId = authorId;
        }
    }

    public class GetByAuthorBookQueryHandler : IRequestHandler<GetByAuthorBookQuery, IList<BookModel>>
    {
        private readonly IBookRepository repository;
        private readonly IAuthorRepository bookAuthorRepository;
        private readonly IMapper mapper;

        public GetByAuthorBookQueryHandler(IBookRepository repository, IAuthorRepository bookAuthorRepository, IMapper mapper)
        {
            this.repository = repository;
            this.bookAuthorRepository = bookAuthorRepository;
            this.mapper = mapper;
        }

        public async Task<IList<BookModel>> Handle(GetByAuthorBookQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ValidationException("Query cannot be null");
            }

            Domain.Entities.Author author = await bookAuthorRepository.BaseRepository.GetById(request.AuthorId);

            if (author == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Author), author.Id);
            }

            IList<Domain.Entities.Book> books = await repository.GetByAuthor(author);

            if (books == null || books.Count == 0)
            {
                return new List<BookModel>();
            }

            IList<BookModel> productModels = mapper.Map<IList<Domain.Entities.Book>, IList<BookModel>>(books);

            return productModels;
        }
    }
}
