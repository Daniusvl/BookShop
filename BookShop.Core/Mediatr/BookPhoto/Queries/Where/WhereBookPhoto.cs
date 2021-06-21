using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.BookPhoto.Queries.Where
{
    public static class WhereBookPhoto
    {
        public record Query(Func<Domain.Entities.BookPhoto, bool> Func) : IRequest<IList<BookPhotoModel>>;

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
                if (request == null)
                {
                    throw new ValidationException("Query cannot be null");
                }

                if (request.Func == null)
                {
                    throw new ValidationException("Func cannot be null");
                }

                IList<Domain.Entities.BookPhoto> bookPhotos = await repository.Where(request.Func);

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
