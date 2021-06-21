using MediatR;

namespace BookShop.Core.Mediatr.Photo.Commands.Delete
{
    public class DeletePhotoCommand : IRequest
    {
        public int Id { get; set; }

        public DeletePhotoCommand(int id)
        {
            Id = id;
        }
    }
}
