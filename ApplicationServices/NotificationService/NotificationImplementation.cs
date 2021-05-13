using ApplicationServices.NotificationService.Windows.Views;

namespace ApplicationServices.NotificationService
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
