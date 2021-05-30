using System;
using System.Windows;
using System.Windows.Input;

namespace ApplicationWPFServices.DebugService
{
    /// <summary>
    /// Логика взаимодействия для DebugWindow.xaml
    /// </summary>
    public partial class DebugWindow : Window
    {
        public DebugWindow() => InitializeComponent();
        public void Log(string message) => DebugTb.Text += message + Environment.NewLine;
        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
