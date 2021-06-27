using BookShop.Core.Models;
using MediatR;
using System;
using System.Collections.Generic;

namespace BookShop.Core.Mediatr.Book.Commands.Update
{
    public class UpdateBookCommand : IRequest<BookModel>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public bool Hidden { get; set; }

        public DateTime DateReleased { get; set; }

        public int AuthorId { get; set; }

        public int CategoryId { get; set; }
    }
}
