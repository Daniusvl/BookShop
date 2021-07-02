using BookShop.CRM.Models;
using BookShop.CRM.Wrappers;
using System;
using System.Globalization;
using System.Windows.Data;

namespace BookShop.CRM
{
    public class CategoryModelWrapperConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is CategoryModel model)
            {
                return new CategoryWrapper(model);
            }
            else if(value is CategoryWrapper wrapper)
            {
                return wrapper.Model;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CategoryModel model)
            {
                return new CategoryWrapper(model);
            }
            else if (value is CategoryWrapper wrapper)
            {
                return wrapper.Model;
            }
            return value;
        }
    }
}
