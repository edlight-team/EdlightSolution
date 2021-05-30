using System;

namespace Styles.Extensions
{
    public static class StringExtension
    {
        public static double ToDouble(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return Convert.ToDouble(str);
            }
            else
            {
                return double.NaN;
            }
        }
        public static DateTime ToDate(this string str)
        {
            DateTime.TryParse(str, out DateTime result);
            return result;
        }
    }
}
