using System;
using System.Globalization;
using System.Windows.Data;

namespace Styles.Converters
{
    public class TimeZoneConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                if (str.Contains(":00"))
                {
                    return false;
                }
            }
            return true;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
