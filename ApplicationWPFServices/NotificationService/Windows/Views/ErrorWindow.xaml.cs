using ApplicationWPFServices.NotificationService.Windows.ViewModels;
using System;
using System.Windows;

namespace ApplicationWPFServices.NotificationService.Windows.Views
{
    /// <summary>
    /// Логика взаимодействия для ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : Window, IDisposable
    {
        public ErrorWindow(string message)
        {
            InitializeComponent();
            DataContext = new ErrorWindowViewModel(message, this);
        }
        public void Dispose() => DataContext = null;
    }
}
