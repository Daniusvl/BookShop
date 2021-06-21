using BookShop.Domain.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace BookShop.Domain.Entities
{
    public class Book : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string FilePath { get; set; } = string.Empty;

        public bool Hidden { get; set; }

        public DateTime DateReleased { get; set; }

        public IList<Photo> Photos { get; set; } = new List<Photo>();

        public Author Author { get; set; }

        public Category Category { get; set; }

        public override string ToString()
        {
            return nameof(Book) + $" Name: {Name}";
        }
    }
}
