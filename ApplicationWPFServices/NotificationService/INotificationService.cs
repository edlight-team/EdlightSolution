namespace ApplicationWPFServices.NotificationService
{
    public interface INotificationService
    {
        void ShowError(string message);
        void ShowGlobalError(string message);
        bool? ShowQuestion(string message);
        void ShowInformation(string message);
        void ShowGlobalInformation(string message);
    }
}
