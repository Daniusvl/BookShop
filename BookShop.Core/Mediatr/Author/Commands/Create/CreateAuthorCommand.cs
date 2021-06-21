using BookShop.Core.Models;
using MediatR;

namespace BookShop.Core.Mediatr.Author.Commands.Create
{
    public class CreateAuthorCommand : IRequest<AuthorModel>
    {
        public string Name { get; set; }
    }
}
