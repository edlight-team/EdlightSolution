using ApplicationServices.HashingService;
using ApplicationServices.WebApiService;
using ApplicationWPFServices.MemoryService;
using ApplicationWPFServices.NotificationService;
using EdlightDesktopClient.ViewModels;
using EdlightDesktopClient.ViewModels.Learn;
using EdlightDesktopClient.ViewModels.Profile;
using EdlightDesktopClient.ViewModels.Schedule;
using EdlightDesktopClient.ViewModels.Settings;
using EdlightDesktopClient.Views;
using EdlightDesktopClient.Views.Learn;
using EdlightDesktopClient.Views.Profile;
using EdlightDesktopClient.Views.Schedule;
using EdlightDesktopClient.Views.Settings;
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
            ViewModelLocationProvider.Register(typeof(LearnMainView).ToString(), () => Container.Resolve<LearnMainViewModel>());
            ViewModelLocationProvider.Register(typeof(ProfileMainView).ToString(), () => Container.Resolve<ProfileMainViewModel>());
            ViewModelLocationProvider.Register(typeof(ScheduleMainView).ToString(), () => Container.Resolve<ScheduleMainViewModel>());
            ViewModelLocationProvider.Register(typeof(SettingsMainView).ToString(), () => Container.Resolve<SettingsMainViewModel>());
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
