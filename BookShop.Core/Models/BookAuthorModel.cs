﻿namespace BookShop.Core.Models
{
    public class BookAuthorModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
