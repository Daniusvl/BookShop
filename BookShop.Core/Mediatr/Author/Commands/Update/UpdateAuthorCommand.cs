using BookShop.Core.Models;
using MediatR;

namespace BookShop.Core.Mediatr.Author.Commands.Update
{
    public class UpdateAuthorCommand : IRequest<AuthorModel>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
