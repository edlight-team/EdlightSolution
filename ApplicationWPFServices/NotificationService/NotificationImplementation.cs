using HandyControl.Controls;

namespace ApplicationWPFServices.NotificationService
{
    public class NotificationImplementation : INotificationService
    {
        public void ShowError(string message) => MessageBox.Show(message, "Ошбика", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        public void ShowGlobalError(string message) => Growl.Error(message, "Global");
        public bool? ShowQuestion(string message)
        {
            System.Windows.MessageBoxResult result = MessageBox.Show(message, "Подтвердите выбор", System.Windows.MessageBoxButton.YesNoCancel, System.Windows.MessageBoxImage.Question);
            if (result == System.Windows.MessageBoxResult.Cancel) return null;
            return result == System.Windows.MessageBoxResult.Yes;
        }
        public void ShowInformation(string message) => MessageBox.Show(message, "Сообщение", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        public void ShowGlobalInformation(string message) => Growl.Info(message, "Global");
    }
}
