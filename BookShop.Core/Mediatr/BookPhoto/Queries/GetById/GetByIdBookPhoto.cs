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

namespace BookShop.Core.Mediatr.BookPhoto.Queries.GetById
{
    public static class GetByIdBookPhoto
    {
        public record Query(int Id) : IRequest<BookPhotoModel>;

        public class Handler : IRequestHandler<Query, BookPhotoModel>
        {
            private readonly IBookPhotoRepository repository;
            private readonly IMapper mapper;

            public Handler(IBookPhotoRepository repository, IMapper mapper)
            {
                this.repository = repository;
                this.mapper = mapper;
            }

            public async Task<BookPhotoModel> Handle(Query request, CancellationToken cancellationToken)
            {
                if (request == null)
                {
                    throw new ValidationException("Query cannot be null");
                }

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
