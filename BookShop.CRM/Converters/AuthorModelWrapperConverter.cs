using BookShop.CRM.Models;
using BookShop.CRM.Wrappers;
using System;
using System.Globalization;
using System.Windows.Data;

namespace BookShop.CRM
{
    public class AuthorModelWrapperConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is AuthorModel model)
            {
                return new AuthorWrapper(model);
            }
            if(value is AuthorWrapper wrapper)
            {
                return wrapper.Model;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AuthorModel model)
            {
                return new AuthorWrapper(model);
            }
            if (value is AuthorWrapper wrapper)
            {
                return wrapper.Model;
            }
            return value;
        }
    }
}
