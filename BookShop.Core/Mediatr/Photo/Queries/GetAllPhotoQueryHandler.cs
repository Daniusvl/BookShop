using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Photo.Queries
{
    public class GetAllPhotoQuery : IRequest<IList<PhotoModel>> { }

    class GetAllPhotoQueryHandler : IRequestHandler<GetAllPhotoQuery, IList<PhotoModel>>
    {
        private readonly IPhotoRepository repository;
        private readonly IMapper mapper;

        public GetAllPhotoQueryHandler(IPhotoRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IList<PhotoModel>> Handle(GetAllPhotoQuery request, CancellationToken cancellationToken)
        {
            IList<Domain.Entities.Photo> photos = await repository.GetAll();

            if (photos == null || photos.Count == 0)
            {
                return new List<PhotoModel>();
            }

            IList<PhotoModel> bookPhotoModels = mapper.Map<IList<Domain.Entities.Photo>, IList<PhotoModel>>(photos);

            return bookPhotoModels;
        }
    }
}
