using BookShop.Core.Abstract.Features.FileUploader;
using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Exceptions;
using BookShop.Domain.Entities;
using System.IO;
using System.Threading.Tasks;

namespace BookShop.Features.FileUploader
{
    public class PdfFileUploader : BaseFileUploader
    {
        private readonly IUnitOfWork unitOfWork;

        public PdfFileUploader(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public override async Task UploadFile(Stream stream, int id, int ContentLength)
        {
            Book book = await unitOfWork.BookRepository.GetById(id);

            if (book == null)
            {
                throw new NotFoundException(nameof(Book), id);
            }

            await Save(stream, ContentLength, book.FilePath);
        }
    }
}
