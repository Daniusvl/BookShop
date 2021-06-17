using BookShop.Domain.Entities.Abstract;

namespace BookShop.Domain.Entities
{
    public class BookPhoto : BaseEntity
    {
        public string FilePath { get; set; } = string.Empty;

        public Product Product { get; set; }

        public override string ToString()
        {
            return nameof(BookPhoto) + $" FilePath: {FilePath}";
        }
    }
}
