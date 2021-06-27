using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Author.Queries
{
    public class GetAllAuthorQuery : IRequest<IList<AuthorModel>> { }

    public class GetAllAuthorQueryHandler : IRequestHandler<GetAllAuthorQuery, IList<AuthorModel>>
    {
        private readonly IAuthorRepository repository;
        private readonly IMapper mapper;

        public GetAllAuthorQueryHandler(IAuthorRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IList<AuthorModel>> Handle(GetAllAuthorQuery request, CancellationToken cancellationToken)
        {
            IList<Domain.Entities.Author> authors = await repository.BaseRepository.GetAll();

            if (authors == null || authors.Count == 0)
            {
                return new List<AuthorModel>();
            }

            IList<AuthorModel> bookAuthorModels = mapper.Map<IList<Domain.Entities.Author>, IList<AuthorModel>>(authors);

            return bookAuthorModels;
        }
    }
}
