using BookShop.Domain.Entities.Abstract;
using System.Collections.Generic;

namespace BookShop.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public IList<Book> Products { get; set; } = new List<Book>();

        public override string ToString()
        {
            return nameof(Category) + $" Name: {Name}";
        }
    }
}
