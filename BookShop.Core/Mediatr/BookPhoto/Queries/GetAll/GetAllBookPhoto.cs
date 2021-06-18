using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Configuration;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            private readonly IConfiguration configuration;
            private readonly IMapper mapper;

            public Handler(IBookPhotoRepository repository, IConfiguration configuration, IMapper mapper)
            {
                this.repository = repository;
                this.configuration = configuration;
                this.mapper = mapper;
            }

            public async Task<IList<BookPhotoModel>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (repository == null)
                {
                    throw new ServiceNullException(nameof(IBookPhotoRepository), nameof(Handler));
                }

                if (configuration == null)
                {
                    throw new ServiceNullException(nameof(IConfiguration), nameof(Handler));
                }

                if (mapper == null)
                {
                    throw new ServiceNullException(nameof(IMapper), nameof(Handler));
                }

                if (configuration.IsDevelopment())
                {
                    if (request == null)
                    {
                        throw new ArgumentNullException(nameof(request));
                    }
                }

                // TODO: Add validation and logging.

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
