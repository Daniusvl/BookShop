using BookShop.Core.Models;
using MediatR;
using System;

namespace BookShop.Core.Mediatr.Book.Commands.Create
{
    public class CreateBookCommand : IRequest<BookModel>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public bool Hidden { get; set; }

        public DateTime DateReleased { get; set; }

        public int AuthorId { get; set; }

        public int CategoryId { get; set; }
    }
}
