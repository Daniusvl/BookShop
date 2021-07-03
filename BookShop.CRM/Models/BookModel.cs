using BookShop.CRM.Core;
using BookShop.CRM.Core.Models;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BookShop.CRM.Models
{
    public class BookModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public bool Hidden { get; set; }

        public DateTime DateReleased { get; set; } = DateTime.UtcNow;

        public IList<PhotoModel> Photos { get; set; }

        public int AuthorId { get; set; }

        public string AuthorName { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public static explicit operator BookModel(AddBookCommand command)
        {
            return new BookModel()
            {
                Name = command.Name,
                Description = command.Description,
                Price = command.Price,
                Hidden = command.Hidden,
                DateReleased = command.DateReleased,
                AuthorId = command.AuthorId,
                CategoryId = command.CategoryId
            };
        }

        public static implicit operator AddBookCommand(BookModel model)
        {
            return new AddBookCommand(model.Name, model.Description, model.Price, model.Hidden, model.DateReleased, model.AuthorId, model.CategoryId);
        }

        public static explicit operator BookModel(UpdateBookCommand command)
        {

            return new BookModel
            {
                Id = command.Id,
                Name = command.Name,
                AuthorId = command.AuthorId,
                CategoryId = command.CategoryId,
                DateReleased = command.DateReleased,
                Description = command.Description,
                Hidden = command.Hidden,
                Price = command.Price
            };
        }

        public static implicit operator UpdateBookCommand(BookModel model)
        {
            return new UpdateBookCommand(model.Id, model.Name, model.Description, model.Price, model.Hidden, model.DateReleased, model.AuthorId, model.CategoryId);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
