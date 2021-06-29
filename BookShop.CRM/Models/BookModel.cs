using System;
using System.Collections.Generic;

namespace BookShop.CRM.Models
{
    public class BookModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public bool Hidden { get; set; }

        public DateTime DateReleased { get; set; }

        public IList<PhotoModel> Photos { get; set; }

        public string AuthorId { get; set; }

        public string AuthorName { get; set; }

        public string CategoryId { get; set; }

        public string CategoryName { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
