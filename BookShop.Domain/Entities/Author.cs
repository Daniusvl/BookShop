﻿using BookShop.Domain.Entities.Abstract;
using System.Collections.Generic;

namespace BookShop.Domain.Entities
{
    public class BookAuthor : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public IList<Product> Products { get; set; } = new List<Product>();

        public override string ToString()
        {
            return nameof(BookAuthor) + $" Name: {Name}";
        }
    }
}
