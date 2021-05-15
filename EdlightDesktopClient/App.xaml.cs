using ApplicationServices.HashingService;
using ApplicationServices.PermissionService;
using ApplicationServices.WebApiService;
using ApplicationWPFServices.MemoryService;
using ApplicationWPFServices.NotificationService;
using EdlightDesktopClient.ViewModels;
using EdlightDesktopClient.ViewModels.Groups;
using EdlightDesktopClient.ViewModels.Learn;
using EdlightDesktopClient.ViewModels.Profile;
using EdlightDesktopClient.ViewModels.Schedule;
using EdlightDesktopClient.ViewModels.Settings;
using EdlightDesktopClient.Views;
using EdlightDesktopClient.Views.Groups;
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
        public App() => Current.Dispatcher.UnhandledException += DispatcherUnhandledExceptionHandler;
        private void DispatcherUnhandledExceptionHandler(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            HandyControl.Controls.Growl.ErrorGlobal(e.Exception.Message);
            e.Handled = true;
        }
        protected override Window CreateShell() => Container.Resolve<AuthWindow>();
        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.Register(typeof(AuthWindow).ToString(), () => Container.Resolve<AuthWindowViewModel>());
            ViewModelLocationProvider.Register(typeof(MainWindow).ToString(), () => Container.Resolve<MainWindowViewModel>());

            ViewModelLocationProvider.Register(typeof(LearnMainView).ToString(), () => Container.Resolve<LearnMainViewModel>());
            ViewModelLocationProvider.Register(typeof(ProfileMainView).ToString(), () => Container.Resolve<ProfileMainViewModel>());
            ViewModelLocationProvider.Register(typeof(ScheduleMainView).ToString(), () => Container.Resolve<ScheduleMainViewModel>());
            ViewModelLocationProvider.Register(typeof(SettingsMainView).ToString(), () => Container.Resolve<SettingsMainViewModel>());
            ViewModelLocationProvider.Register(typeof(GroupsMainView).ToString(), () => Container.Resolve<GroupsMainViewModel>());

            ViewModelLocationProvider.Register(typeof(ScheduleDateViewer).ToString(), () => Container.Resolve<ScheduleDateViewerViewModel>());
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IHashingService, HashingImplementation>();
            containerRegistry.RegisterSingleton<IMemoryService, MemoryImplementation>();
            containerRegistry.RegisterSingleton<INotificationService, NotificationImplementation>();
            containerRegistry.RegisterSingleton<IWebApiService, WebApiServiceImplementation>();
            containerRegistry.RegisterSingleton<IPermissionService, PermissionImplementation>();
        }
    }
}
