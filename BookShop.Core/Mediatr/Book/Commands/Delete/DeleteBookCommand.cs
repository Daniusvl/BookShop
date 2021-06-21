using MediatR;

namespace BookShop.Core.Mediatr.Book.Commands.Delete
{
    public class DeleteBookCommand : IRequest
    {
        public int Id { get; set; }

        public DeleteBookCommand(int id)
        {
            Id = id;
        }
    }
}
