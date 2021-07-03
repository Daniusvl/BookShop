using BookShop.Core.Models;
using MediatR;

namespace BookShop.Core.Mediatr.Photo.Commands.Create
{
    public class CreatePhotoCommand : IRequest<PhotoModel>
    {
        public int BookId { get; set; }
    }
}
