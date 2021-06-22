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
    public class GetByCategoryBookQuery : IRequest<IList<BookModel>>
    {
        public int CategoryId { get; set; }

        public GetByCategoryBookQuery(int categoryId)
        {
            CategoryId = categoryId;
        }
    }

    public class GetByCategoryBookQueryHandler : IRequestHandler<GetByCategoryBookQuery, IList<BookModel>>
    {
        private readonly IBookRepository repository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public GetByCategoryBookQueryHandler(IBookRepository repository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            this.repository = repository;
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        public async Task<IList<BookModel>> Handle(GetByCategoryBookQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ValidationException("Query cannot be null");
            }

            Domain.Entities.Category category = await categoryRepository.GetById(request.CategoryId);

            if (category == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Category), request.CategoryId);
            }

            IList<Domain.Entities.Book> books = repository.GetByCategory(category);

            if (books == null || books.Count == 0)
            {
                return new List<BookModel>();
            }

            IList<BookModel> productModels = mapper.Map<IList<Domain.Entities.Book>, IList<BookModel>>(books);

            return productModels;
        }
    }
}
