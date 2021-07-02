using BookShop.CRM.Core;

namespace BookShop.CRM.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public static explicit operator CategoryModel(AddCategoryCommand command)
        {
            return new CategoryModel { Name = command.Name };
        }

        public static implicit operator AddCategoryCommand(CategoryModel model)
        {
            return new AddCategoryCommand(model.Name);
        }

        public static explicit operator CategoryModel(UpdateCategoryCommand command)
        {
            return new CategoryModel {Id = command.Id, Name = command.Name };
        }

        public static implicit operator UpdateCategoryCommand(CategoryModel model)
        {
            return new UpdateCategoryCommand(model.Id, model.Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
