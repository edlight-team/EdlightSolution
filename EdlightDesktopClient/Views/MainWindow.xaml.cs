using System.Windows;
using System.Windows.Input;

namespace EdlightDesktopClient.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Height = SystemParameters.FullPrimaryScreenHeight;
        }
        private void BorderMouseDown(object sender, MouseButtonEventArgs e) => DragMove();
    }
}
