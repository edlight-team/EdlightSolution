using HandyControl.Controls;

namespace ApplicationWPFServices.NotificationService
{
    public class NotificationImplementation : INotificationService
    {
        public void ShowError(string message) => MessageBox.Show(message, "Ошбика", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        public void ShowGlobalError(string message) => Growl.Error(message, "Global");
    }
}
