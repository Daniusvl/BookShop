using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Core.Mediatr.Photo.Commands.Create;
using BookShop.Core.Models;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Photo.Commands.Update
{
    public class UpdatePhotoCommandHandler : IRequestHandler<UpdatePhotoCommand, PhotoModel>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<CreatePhotoCommandHandler> logger;

        public UpdatePhotoCommandHandler(IUnitOfWork unitOfWork,
            IMapper mapper, ILogger<CreatePhotoCommandHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<PhotoModel> Handle(UpdatePhotoCommand request, CancellationToken cancellationToken)
        {
            UpdatePhotoRequestValidator validator = new();
            ValidationResult result = await validator.ValidateAsync(request);
            
            if (result.Errors.Count > 0)
            {
                throw new ValidationException(result);
            }

            Domain.Entities.Photo photo = await unitOfWork.PhotoRepository.BaseRepository.GetById(request.Id);

            if(photo == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Photo), request.Id);
            }

            Domain.Entities.Book book = await unitOfWork.BookRepository.GetById(request.BookId);

            if (book == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Book), request.BookId);
            }

            photo.BookId = request.BookId;

            await unitOfWork.PhotoRepository.BaseRepository.Update(photo);

            logger.LogInformation($"{nameof(Domain.Entities.Photo)} with Id: {book.Id} modified by {book.LastModifiedBy} at {book.DateLastModified}");

            PhotoModel model = mapper.Map<Domain.Entities.Photo, PhotoModel>(photo);

            return model;
        }
    }
}
