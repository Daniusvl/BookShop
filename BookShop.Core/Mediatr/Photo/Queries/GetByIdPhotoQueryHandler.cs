using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Photo.Queries
{
    public class GetByIdPhotoQuery : IRequest<PhotoModel>
    {
        public int Id { get; set; }

        public GetByIdPhotoQuery(int id)
        {
            Id = id;
        }
    }

    public class GetByIdPhotoQueryHandler : IRequestHandler<GetByIdPhotoQuery, PhotoModel>
    {
        private readonly IPhotoRepository repository;
        private readonly IMapper mapper;

        public GetByIdPhotoQueryHandler(IPhotoRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<PhotoModel> Handle(GetByIdPhotoQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ValidationException("Query cannot be null");
            }

            Domain.Entities.Photo photo = await repository.BaseRepository.GetById(request.Id);

            if (photo == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Photo), request.Id);
            }

            PhotoModel bookPhotoModel = mapper.Map<Domain.Entities.Photo, PhotoModel>(photo);

            return bookPhotoModel;
        }
    }
}
