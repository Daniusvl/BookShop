using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Core.Mediatr.Photo.Commands.Create
{
    public class CreatePhotoCommandHandler : IRequestHandler<CreatePhotoCommand, PhotoModel>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<CreatePhotoCommandHandler> logger;

        public CreatePhotoCommandHandler(IUnitOfWork unitOfWork, 
            IMapper mapper, ILogger<CreatePhotoCommandHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<PhotoModel> Handle(CreatePhotoCommand request, CancellationToken cancellationToken)
        {
            CreatePhotoRequestValidator validator = new();
            ValidationResult result = await validator.ValidateAsync(request);

            if (result.Errors.Count > 0)
            {
                throw new ValidationException(result);
            }

            Domain.Entities.Book book = await unitOfWork.BookRepository.GetById(request.BookId);

            if(book == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Book), request.BookId);
            }

            if (!Directory.Exists(Directory.GetCurrentDirectory() + @$"\Photos\{book.Name}"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @$"\Photos\{book.Name}");
            }

            string path = Directory.GetCurrentDirectory() + @$"\Photos\{book.Name}\{Guid.NewGuid()}.png";

            int i = 0;
            while (File.Exists(path))
            {
                path = Directory.GetCurrentDirectory() + @$"\Photos\{book.Name}\{Guid.NewGuid()}.png";
                i++;
            }

            if (i >= 5)
            {
                logger.LogWarning("Photo file name generation takes more than 4 iterations");
            }

            Domain.Entities.Photo photo = new()
            {
                FilePath = path,
                BookId = book.Id
            };

            await unitOfWork.PhotoRepository.BaseRepository.Create(photo);

            logger.LogInformation($"{nameof(Domain.Entities.Photo)} with Id: {photo.Id} created by {photo.CreatedBy} at {photo.DateCreated}");

            return mapper.Map<Domain.Entities.Photo, PhotoModel>(photo);
        }
    }
}
