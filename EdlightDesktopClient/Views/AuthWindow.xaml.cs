using System.Windows;
using System.Windows.Input;

namespace EdlightDesktopClient.Views
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow() => InitializeComponent();
        private void BorderMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed) return;
            DragMove();
        }
    }
}
