using BookShop.Core.Models;
using MediatR;

namespace BookShop.Core.Mediatr.Category.Commands.Create
{
    public class CreateCategoryCommand : IRequest<CategoryModel>
    {
        public string Name { get; set; }
    }
}
