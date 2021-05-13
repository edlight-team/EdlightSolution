using ApplicationWPFServices.NotificationService.Windows.Views;

namespace ApplicationWPFServices.NotificationService
{
    public class NotificationImplementation : INotificationService
    {
        public void ShowError(string message)
        {
            using ErrorWindow window = new(message);
            window.ShowDialog();
        }
    }
}
