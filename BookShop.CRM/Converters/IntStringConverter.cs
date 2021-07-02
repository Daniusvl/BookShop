using System;
using System.Globalization;
using System.Windows.Data;

namespace BookShop.CRM
{
    public class IntStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string s)
            {
                bool parsed = int.TryParse(s, out int res);
                if (parsed) return res;
                else return 0;
            }
            if (value is int i)
            {
                return i.ToString();
            }
            else return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                bool parsed = int.TryParse(s, out int res);
                if (parsed) return res;
                else return 0;
            }
            if (value is int i)
            {
                return i.ToString();
            }
            else return value;
        }
    }
}
