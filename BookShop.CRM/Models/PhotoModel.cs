using BookShop.CRM.Core;

namespace BookShop.CRM.Models
{
    public class PhotoModel
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public static explicit operator PhotoModel(AddPhotoCommand command)
        {
            return new PhotoModel { BookId = command.BookId };
        }

        public static implicit operator AddPhotoCommand(PhotoModel model)
        {
            return new(model.BookId);
        }

        public static explicit operator PhotoModel(UpdatePhotoCommand command)
        {
            return new PhotoModel { BookId = command.BookId };
        }

        public static implicit operator UpdatePhotoCommand(PhotoModel model)
        {
            return new(model.Id, model.BookId);
        }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
