using BookShop.CRM.Models;
using BookShop.CRM.Wrappers;
using System;
using System.Globalization;
using System.Windows.Data;

namespace BookShop.CRM
{
    public class PhotoModelWrapperConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is PhotoModel model)
            {
                return new PhotoWrapper(model);
            }
            if(value is PhotoWrapper wrapper)
            {
                return wrapper.Model;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PhotoModel model)
            {
                return new PhotoWrapper(model);
            }
            if (value is PhotoWrapper wrapper)
            {
                return wrapper.Model;
            }
            return value;
        }
    }
}
