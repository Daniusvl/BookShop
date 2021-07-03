using BookShop.CRM.Models;
using BookShop.CRM.Wrappers.Base;
using System.Collections.Generic;

namespace BookShop.CRM.Wrappers
{
    public class PhotoWrapper : ModelWrapper<PhotoModel>
    {
        public PhotoWrapper(PhotoModel model) : base(model, false)
        {

        }

        public int Id
        {
            get => GetValue<int>(nameof(Id));
            set => SetValue(value, nameof(Id));
        }

        public int BookId
        {
            get => GetValue<int>(nameof(BookId));
            set => SetValue(value, nameof(BookId));
        }

        protected override IEnumerable<string> ValidateProperties(string propertyName)
        {
            return new List<string>();
        }
    }
}
