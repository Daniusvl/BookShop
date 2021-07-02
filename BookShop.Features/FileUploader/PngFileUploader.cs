using BookShop.Core.Abstract.Features.FileUploader;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Domain.Entities;
using System.IO;
using System.Threading.Tasks;

namespace BookShop.Features.FileUploader
{
    public class PngFileUploader : BaseFileUploader
    {
        private readonly IUnitOfWork unitOfWork;

        public PngFileUploader(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public override async Task UploadFile(Stream stream, int id, int ContentLength)
        {
            Photo photo = await unitOfWork.PhotoRepository.BaseRepository.GetById(id);

            if(photo == null)
            {
                throw new NotFoundException(nameof(Photo), id);
            }

            await Save(stream, ContentLength, photo.FilePath);
        }
    }
}
