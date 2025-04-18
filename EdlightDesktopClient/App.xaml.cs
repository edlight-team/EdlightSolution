﻿using ApplicationServices.HashingService;
using ApplicationServices.IdentificatorService;
using ApplicationServices.PermissionService;
using ApplicationServices.SignalClientSerivce;
using ApplicationServices.TranslationService;
using ApplicationServices.WebApiService;
using ApplicationWPFServices.DebugService;
using ApplicationWPFServices.MemoryService;
using ApplicationWPFServices.NotificationService;
using EdlightDesktopClient.ViewModels;
using EdlightDesktopClient.ViewModels.Dictionaries;
using EdlightDesktopClient.ViewModels.Groups;
using EdlightDesktopClient.ViewModels.Learn;
using EdlightDesktopClient.ViewModels.Profile;
using EdlightDesktopClient.ViewModels.Schedule;
using EdlightDesktopClient.ViewModels.Schedule.CapacityWindows;
using EdlightDesktopClient.ViewModels.Schedule.EditSchedule;
using EdlightDesktopClient.ViewModels.Settings;
using EdlightDesktopClient.Views;
using EdlightDesktopClient.Views.Dictionaries;
using EdlightDesktopClient.Views.Groups;
using EdlightDesktopClient.Views.Learn;
using EdlightDesktopClient.Views.Profile;
using EdlightDesktopClient.Views.Schedule;
using EdlightDesktopClient.Views.Schedule.CapacityWindows;
using EdlightDesktopClient.Views.Schedule.EditSchedule;
using EdlightDesktopClient.Views.Settings;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;
using System.Windows;

namespace EdlightDesktopClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            Current.Dispatcher.UnhandledException += DispatcherUnhandledExceptionHandler;
            return Container.Resolve<AuthWindow>();
        }
        private void DispatcherUnhandledExceptionHandler(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            HandyControl.Controls.Growl.Error(e.Exception.Message, "Global");
            e.Handled = true;
        }
        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.Register<AuthWindow, AuthWindowViewModel>();
            ViewModelLocationProvider.Register<MainWindow, MainWindowViewModel>();
            ViewModelLocationProvider.Register<LearnMainView, LearnMainViewModel>();
            ViewModelLocationProvider.Register<ProfileMainView, ProfileMainViewModel>();
            ViewModelLocationProvider.Register<SettingsMainView, SettingsMainViewModel>();
            ViewModelLocationProvider.Register<GroupsMainView, GroupsMainViewModel>();
            ViewModelLocationProvider.Register<AddDeleteGroupView, AddDeleteGroupViewModel>();
            ViewModelLocationProvider.Register<ScheduleMainView, ScheduleMainViewModel>();
            ViewModelLocationProvider.Register<ScheduleDateViewer, ScheduleDateViewerViewModel>();
            ViewModelLocationProvider.Register<CapacityManagmentView, CapacityManagmentViewModel>();
            ViewModelLocationProvider.Register<AddScheduleView, AddScheduleViewModel>();
            ViewModelLocationProvider.Register<AddRecoursiveScheduleView, AddRecoursiveScheduleViewModel>();
            ViewModelLocationProvider.Register<CancelScheduleRecordView, CancelScheduleRecordViewModel>();
            ViewModelLocationProvider.Register<DictionariesMainView, DictionariesMainViewModel>();
            ViewModelLocationProvider.Register<EditDisciplinesView, EditDisciplinesViewModel>();
            ViewModelLocationProvider.Register<EditAudienceView, EditAudienceViewModel>();
            ViewModelLocationProvider.Register<EditTeacherView, EditTeacherViewModel>();
            ViewModelLocationProvider.Register<EditLearnPlanView, EditLearnPlanViewModel>();
            ViewModelLocationProvider.Register<TestListView, TestListViewModel>();
            ViewModelLocationProvider.Register<AddTestView, AddTestViewModel>();
            ViewModelLocationProvider.Register<PassingTestView, PassingTestViewModel>();
            ViewModelLocationProvider.Register<ResultsTestView, ResultsTestViewModel>();
            ViewModelLocationProvider.Register<StorageListView, StorageListViewModel>();
            ViewModelLocationProvider.Register<StorageFileListView, StorageFileListViewModel>();
            ViewModelLocationProvider.Register<AddStorageView, AddStorageViewModel>();
            ViewModelLocationProvider.Register<FileListFiew, FileListViewModel>();
            ViewModelLocationProvider.Register<AddManualFIleView, AddManualFIleViewModel>();

            ViewModelLocationProvider.Register<CreateTeacherWindow, CreateTeacherWindowViewModel>();
            ViewModelLocationProvider.Register<CreateDisciplineWindow, CreateDisciplineWindowViewModel>();
            ViewModelLocationProvider.Register<CreateRecoursiveScheduleWindow, CreateRecoursiveScheduleWindowViewModel>();
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<LearnMainView>();
            containerRegistry.RegisterForNavigation<ProfileMainView>();
            containerRegistry.RegisterForNavigation<SettingsMainView>();
            containerRegistry.RegisterForNavigation<GroupsMainView>();
            containerRegistry.RegisterForNavigation<AddDeleteGroupView>();
            containerRegistry.RegisterForNavigation<ScheduleMainView>();
            containerRegistry.RegisterForNavigation<ScheduleDateViewer>();
            containerRegistry.RegisterForNavigation<CapacityManagmentView>();
            containerRegistry.RegisterForNavigation<AddScheduleView>();
            containerRegistry.RegisterForNavigation<AddRecoursiveScheduleView>();
            containerRegistry.RegisterForNavigation<CancelScheduleRecordView>();
            containerRegistry.RegisterForNavigation<DictionariesMainView>();
            containerRegistry.RegisterForNavigation<EditDisciplinesView>();
            containerRegistry.RegisterForNavigation<EditAudienceView>();
            containerRegistry.RegisterForNavigation<EditTeacherView>();
            containerRegistry.RegisterForNavigation<EditLearnPlanView>();
            containerRegistry.RegisterForNavigation<TestListView>();
            containerRegistry.RegisterForNavigation<AddTestView>();
            containerRegistry.RegisterForNavigation<PassingTestView>();
            containerRegistry.RegisterForNavigation<ResultsTestView>();
            containerRegistry.RegisterForNavigation<StorageListView>();
            containerRegistry.RegisterForNavigation<StorageFileListView>();
            containerRegistry.RegisterForNavigation<AddStorageView>();
            containerRegistry.RegisterForNavigation<FileListFiew>();
            containerRegistry.RegisterForNavigation<AddManualFIleView>();

            containerRegistry.RegisterDialog<CreateTeacherWindow, CreateTeacherWindowViewModel>(nameof(CreateTeacherWindow));
            containerRegistry.RegisterDialog<CreateDisciplineWindow, CreateDisciplineWindowViewModel>(nameof(CreateDisciplineWindow));
            containerRegistry.RegisterDialog<CreateRecoursiveScheduleWindow, CreateRecoursiveScheduleWindowViewModel>(nameof(CreateRecoursiveScheduleWindow));

            containerRegistry.RegisterSingleton<IHashingService, HashingImplementation>();
            containerRegistry.RegisterSingleton<IIdentificatorService, IdentificatorImplementation>();
            containerRegistry.RegisterSingleton<IMemoryService, MemoryImplementation>();
            containerRegistry.RegisterSingleton<INotificationService, NotificationImplementation>();
            containerRegistry.RegisterSingleton<ISignalClientService, SignalClientImplementation>();
            containerRegistry.RegisterSingleton<ITranslationService, TranslationImplementation>();
            containerRegistry.RegisterSingleton<IWebApiService, WebApiServiceImplementation>();
            containerRegistry.RegisterSingleton<IPermissionService, PermissionImplementation>();
            containerRegistry.RegisterSingleton<IDebugService, DebugImplementation>();
        }
    }
}
