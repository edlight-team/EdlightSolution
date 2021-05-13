using ApplicationServices.HashingService;
using ApplicationServices.MemoryService;
using ApplicationServices.NotificationService;
using ApplicationServices.WebApiService;
using EdlightDesktopClient.ViewModels;
using EdlightDesktopClient.Views;
using Prism.Ioc;
using Prism.Mvvm;
using System.Windows;

namespace EdlightDesktopClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<AuthWindow>();
        }
        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
            ViewModelLocationProvider.Register(typeof(AuthWindow).ToString(), () => Container.Resolve<AuthWindowViewModel>());
            ViewModelLocationProvider.Register(typeof(MainWindow).ToString(), () => Container.Resolve<MainWindowViewModel>());
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IHashingService, HashingImplementation>();
            containerRegistry.RegisterSingleton<IMemoryService, MemoryImplementation>();
            containerRegistry.RegisterSingleton<INotificationService, NotificationImplementation>();
            containerRegistry.RegisterSingleton<IWebApiService, WebApiServiceImplementation>();
        }
    }
}
