using System.Threading.Tasks;
using System.Windows;

namespace ApplicationWPFServices.DebugService
{
    public class DebugImplementation : IDebugService
    {
        private static DebugWindow window;
        public void ConfigureDebugWindow()
        {
            window = new DebugWindow();
            //window.Show();
        }
        public async void Log(string message)
        {
            if (window.Visibility == Visibility.Collapsed) return;
            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    window?.Log(message);
                });
            });
        }
        public void Clear() => window?.DebugTb?.Clear();
    }
}
