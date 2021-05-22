using ApplicationServices.HashingService;
using ApplicationServices.PermissionService;
using ApplicationServices.WebApiService;
using ApplicationXamarinService.MemoryService;
using EdlightMobileClient.ViewModels;
using EdlightMobileClient.ViewModels.EducationViewModels;
using EdlightMobileClient.ViewModels.ScheduleViewModels;
using EdlightMobileClient.ViewModels.Shell;
using EdlightMobileClient.Views;
using EdlightMobileClient.Views.EducationViews;
using EdlightMobileClient.Views.ScheduleViews;
using EdlightMobileClient.Views.Shell;
using Prism;
using Prism.Ioc;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace EdlightMobileClient
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }
        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/AuthPage");
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.Register<IMemoryService, MemoryImplementation>();
            containerRegistry.Register<IPermissionService, PermissionImplementation>();
            containerRegistry.Register<IHashingService, HashingImplementation>();
            containerRegistry.Register<IWebApiService, WebApiServiceImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<AuthPage, AuthPageViewModel>();
            containerRegistry.RegisterForNavigation<ShellTabbedPage, ShellTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<WeekSchedulePage, WeekSchedulePageViewModel>();
            containerRegistry.RegisterForNavigation<DaySchedulePage, DaySchedulePageViewModel>();
            containerRegistry.RegisterForNavigation<ListTestsPage, ListTestsPageViewModel>();
            containerRegistry.RegisterForNavigation<StartEndTestPage, StartEndTestPageViewModel>();
            containerRegistry.RegisterForNavigation<TestingPage, TestingPageViewModel>();
            containerRegistry.RegisterForNavigation<ListTestResult, ListTestResultViewModel>();
        }
    }
}
