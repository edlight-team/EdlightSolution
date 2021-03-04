using Xamarin.Forms;
using XamarinClient.Views;

namespace XamarinClient
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new ShellView();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
