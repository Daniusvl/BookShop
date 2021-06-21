using MediatR;

namespace BookShop.Core.Mediatr.Author.Commands.Delete
{
    public class DeleteAuthorCommand : IRequest
    {
        public int Id { get; set; }

        public DeleteAuthorCommand(int id)
        {
            Id = id;
        }
    }
}
