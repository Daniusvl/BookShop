using System;

namespace BookShop.CRM.Core.Models
{
    public class AddBookCommand
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
