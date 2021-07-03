using BookShop.Core.Models;
using MediatR;

namespace BookShop.Core.Mediatr.Photo.Commands.Update
{
    public class UpdatePhotoCommand : IRequest<PhotoModel>
    {
        public int Id { get; set; }

        public int BookId { get; set; }
    }
}
