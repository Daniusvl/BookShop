namespace BookShop.Core.Models
{
    public class AuthorModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
