using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Author.Queries
{
    public class GetByIdAuthorQuery : IRequest<AuthorModel> 
    {
        public int Id { get; set; }

        public GetByIdAuthorQuery(int id)
        {
            Id = id;
        }
    }

    public class GetByIdAuthorQueryHandler : IRequestHandler<GetByIdAuthorQuery, AuthorModel>
    {
        private readonly IAuthorRepository repository;
        private readonly IMapper mapper;

        public GetByIdAuthorQueryHandler(IAuthorRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<AuthorModel> Handle(GetByIdAuthorQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ValidationException("Query cannot be null");
            }

            Domain.Entities.Author entity = await repository.BaseRepository.GetById(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Author), request.Id);
            }

            AuthorModel model = mapper.Map<Domain.Entities.Author, AuthorModel>(entity);

            return model;
        }
    }
}
