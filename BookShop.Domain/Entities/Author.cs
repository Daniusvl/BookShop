using BookShop.Domain.Entities.Abstract;
using System.Collections.Generic;

namespace BookShop.Domain.Entities
{
    public class Author : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public IList<Book> Books { get; set; } = new List<Book>();

        public override string ToString()
        {
            return nameof(Author) + $" Name: {Name}";
        }
    }
}
