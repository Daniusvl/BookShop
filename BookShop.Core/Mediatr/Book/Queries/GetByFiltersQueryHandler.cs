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
    public class GetByFiltersQuery : IRequest<IList<BookModel>> 
    { 
        public string CategoryName { get; set; }

        public string AuthorName { get; set; }

        public decimal PriceMin { get; set; }

        public decimal PriceMax { get; set; }
    }

    public class GetByFiltersQueryHandler : IRequestHandler<GetByFiltersQuery, IList<BookModel>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public GetByFiltersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IList<BookModel>> Handle(GetByFiltersQuery request, CancellationToken cancellationToken)
        {
            IList<Domain.Entities.Author> authors = await unitOfWork.AuthorRepository.SearchByName(request.AuthorName);

            if(authors == null || authors.Count == 0)
            {
                throw new NotFoundException(nameof(Domain.Entities.Author), request.AuthorName);
            }

            IList<Domain.Entities.Category> categories = await unitOfWork.CategoryRepository.SearchByName(request.CategoryName);

            if (categories == null || categories.Count == 0)
            {
                throw new NotFoundException(nameof(Domain.Entities.Category), request.CategoryName);
            }

            IList<Domain.Entities.Book> books = await unitOfWork.BookRepository.GetByFilters(categories, authors, request.PriceMin, request.PriceMax);

            if (books == null || books.Count == 0)
            {
                return new List<BookModel>();
            }

            IList<BookModel> models = mapper.Map<IList<Domain.Entities.Book>, IList<BookModel>>(books);

            return models;
        }
    }
}
