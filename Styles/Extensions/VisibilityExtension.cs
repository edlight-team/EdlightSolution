using System.Windows;

namespace Styles.Extensions
{
    public static class VisibilityExtension
    {
        public static bool ToBoolean(this Visibility visibility)
        {
            if (visibility == Visibility.Visible)
                return true;
            else
                return false;
        }
    }
}
