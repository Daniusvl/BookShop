using AutoMapper;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Core.Models;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
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
            CreatePhotoRequestValidator validator = new(unitOfWork.BookRepository);
            ValidationResult result = await validator.ValidateAsync(request);

            if (result.Errors.Count > 0)
            {
                throw new ValidationException(result);
            }

            if (!Directory.Exists(Directory.GetCurrentDirectory() + @$"\Photos\{request.ProductName}"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @$"\Photos\{request.ProductName}");
            }

            string path = Directory.GetCurrentDirectory() + @$"\Photos\{request.ProductName}\{Guid.NewGuid()}.png";

            int i = 0;
            while (File.Exists(path))
            {
                path = Directory.GetCurrentDirectory() + @$"\Photos\{request.ProductName}\{Guid.NewGuid()}.png";
                i++;
            }

            if (i >= 5)
            {
                logger.LogWarning("Photo file name generation takes more than 4 iterations");
            }

            await File.WriteAllBytesAsync(path, request.FileBytes.ToArray());

            Domain.Entities.Photo photo = new()
            {
                FilePath = path,
                BookId = (await unitOfWork.BookRepository.GetByName(request.ProductName)).Id
            };

            await unitOfWork.PhotoRepository.BaseRepository.Create(photo);

            logger.LogInformation($"{nameof(Domain.Entities.Photo)} with Id: {photo.Id} created by {photo.CreatedBy} at {photo.DateCreated}");

            return mapper.Map<Domain.Entities.Photo, PhotoModel>(photo);
        }
    }
}
