using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Styles.Converters
{
    public class IntegerToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int.TryParse(parameter.ToString(), out int count_parameter);
            if (value is int count_value)
            {
                if (count_parameter == count_value) return Visibility.Collapsed;
                else return Visibility.Visible;
            }
            else return Visibility.Visible;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
