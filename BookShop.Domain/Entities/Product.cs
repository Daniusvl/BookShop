using BookShop.Domain.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace BookShop.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string FilePath { get; set; } = string.Empty;

        public bool Hidden { get; set; }

        public DateTime DateReleased { get; set; }

        public IList<BookPhoto> Photos { get; set; } = new List<BookPhoto>();

        public Author Author { get; set; }

        public Category Category { get; set; }

        public override string ToString()
        {
            return nameof(Product) + $" Name: {Name}";
        }
    }
}
