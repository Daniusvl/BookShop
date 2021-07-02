using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace BookShop.CRM
{
    public class DecimalStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                if (s.Length > 1 && s.Last() == '.') s += "0";
                bool parsed = decimal.TryParse(s, out decimal res);
                if (parsed) return res;
                else return 0;
            }
            if (value is decimal d)
            {
                return d.ToString();
            }
            else return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                if (s.Length > 1 && s.Last() == '.') s += "0";
                bool parsed = decimal.TryParse(s, out decimal res);
                if (parsed) return res;
                else return 0;
            }
            if (value is decimal d)
            {
                return d.ToString();
            }
            else return value;
        }
    }
}
