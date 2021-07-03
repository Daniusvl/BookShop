namespace BookShop.CRM.Models
{
    public class PhotoModel
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
