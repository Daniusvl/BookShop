using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using MediatR;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.BookPhoto.Queries.GetAll
{
    public static class GetAllBookPhoto
    {
        public record Query() : IRequest<IList<BookPhotoModel>>;

        public class Handler : IRequestHandler<Query, IList<BookPhotoModel>>
        {
            private readonly IBookPhotoRepository repository;
            private readonly IMapper mapper;

            public Handler(IBookPhotoRepository repository, IMapper mapper)
            {
                this.repository = repository;
                this.mapper = mapper;
            }

            public async Task<IList<BookPhotoModel>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (repository == null)
                {
                    throw new ServiceNullException(nameof(IBookPhotoRepository), nameof(Handler));
                }

                if (mapper == null)
                {
                    throw new ServiceNullException(nameof(IMapper), nameof(Handler));
                }

                IList<Domain.Entities.BookPhoto> bookPhotos = await repository.GetAll();

                if(bookPhotos == null || bookPhotos.Count == 0)
                {
                    return new List<BookPhotoModel>();
                }

                IList<BookPhotoModel> bookPhotoModels = mapper.Map<IList<Domain.Entities.BookPhoto>, IList<BookPhotoModel>>(bookPhotos);

                return bookPhotoModels;
            }
        }

    }
}
