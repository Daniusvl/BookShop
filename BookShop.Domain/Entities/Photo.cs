using BookShop.Domain.Entities.Abstract;

namespace BookShop.Domain.Entities
{
    public class Photo : BaseEntity
    {
        public string FilePath { get; set; } = string.Empty;

        public Book Product { get; set; }

        public override string ToString()
        {
            return nameof(Photo) + $" FilePath: {FilePath}";
        }
    }
}
