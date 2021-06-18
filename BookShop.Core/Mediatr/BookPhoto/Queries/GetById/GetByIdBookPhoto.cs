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

namespace BookShop.Core.Mediatr.BookPhoto.Queries.GetById
{
    public static class GetByIdBookPhoto
    {
        public record Query(int Id) : IRequest<BookPhotoModel>;

        public class Handler : IRequestHandler<Query, BookPhotoModel>
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

            public async Task<BookPhotoModel> Handle(Query request, CancellationToken cancellationToken)
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

                Domain.Entities.BookPhoto bookPhoto = await repository.GetById(request.Id);

                if(bookPhoto == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.BookPhoto), request.Id);
                }

                BookPhotoModel bookPhotoModel = mapper.Map<Domain.Entities.BookPhoto, BookPhotoModel>(bookPhoto);

                return bookPhotoModel;
            }
        }
    }
}
