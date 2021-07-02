using BookShop.CRM.Core;

namespace BookShop.CRM.Models
{
    public class AuthorModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public static explicit operator AuthorModel(AddAuthorCommand command)
        {
            return new AuthorModel() { Name = command.Name };
        }

        public static implicit operator AddAuthorCommand(AuthorModel model)
        {
            return new AddAuthorCommand(model.Name);
        }

        public static explicit operator AuthorModel(UpdateAuthorCommand command)
        {
            return new AuthorModel { Id = command.Id, Name = command.Name };
        }

        public static implicit operator UpdateAuthorCommand(AuthorModel model)
        {
            return new UpdateAuthorCommand(model.Id, model.Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
