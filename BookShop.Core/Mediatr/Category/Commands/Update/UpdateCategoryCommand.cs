using BookShop.Core.Models;
using MediatR;

namespace BookShop.Core.Mediatr.Category.Commands.Update
{
    public class UpdateCategoryCommand : IRequest<CategoryModel>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
