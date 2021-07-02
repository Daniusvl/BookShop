using BookShop.CRM.Models;
using BookShop.CRM.Wrappers;
using System;
using System.Globalization;
using System.Windows.Data;

namespace BookShop.CRM
{
    public class BookModelWrapperConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is BookModel model)
            {
                return new BookWrapper(model);
            }
            if (value is BookWrapper wrapper)
            {
                return wrapper.Model;
            }
            else return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BookModel model)
            {
                return new BookWrapper(model);
            }
            if (value is BookWrapper wrapper)
            {
                return wrapper.Model;
            }
            else return value;
        }
    }
}
