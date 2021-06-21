using MediatR;

namespace BookShop.Core.Mediatr.Category.Commands.Delete
{
    public class DeleteCategoryCommand : IRequest
    {
        public int Id { get; set; }

        public DeleteCategoryCommand(int id)
        {
            Id = id;
        }
    }
}
