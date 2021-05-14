using ApplicationWPFServices.MemoryService;
using EdlightDesktopClient.BaseMethods;
using EdlightDesktopClient.Views.Learn;
using EdlightDesktopClient.Views.Profile;
using EdlightDesktopClient.Views.Schedule;
using EdlightDesktopClient.Views.Settings;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Styles.Models;
using System;
using System.Windows;
using Unity;

namespace EdlightDesktopClient.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region services

        private readonly IRegionManager manager;

        #endregion
        #region fields

        private string _title = "Edlight";
        private WindowState _currentState;
        private LoaderModel _loader;

        #endregion
        #region props

        public string Title { get => _title; set => SetProperty(ref _title, value); }
        public WindowState CurrentState { get => _currentState; set => SetProperty(ref _currentState, value); }
        public LoaderModel Loader { get => _loader; set => SetProperty(ref _loader, value); }

        #endregion
        #region commands

        public DelegateCommand MinimizeCommand { get; private set; }
        public DelegateCommand CloseCommand { get; private set; }
        public DelegateCommand LoadedCommand { get; private set; }

        #endregion
        #region ctor

        public MainWindowViewModel(IUnityContainer container, IRegionManager manager)
        {
            Loader = new();
            this.manager = manager;

            MinimizeCommand = new DelegateCommand(() => CurrentState = StaticCommands.ChangeWindowState(CurrentState));
            CloseCommand = new DelegateCommand(StaticCommands.Shutdown);
            LoadedCommand = new DelegateCommand(OnLoaded);

            container.RegisterType<object, LearnMainView>(nameof(LearnMainView));
            container.RegisterType<object, ProfileMainView>(nameof(ProfileMainView));
            container.RegisterType<object, ProfileMainView>(nameof(ProfileMainView));
            container.RegisterType<object, SettingsMainView>(nameof(SettingsMainView));

            manager.RegisterViewWithRegion(BaseMethods.RegionNames.LearnRegion, typeof(LearnMainView));
            manager.RegisterViewWithRegion(BaseMethods.RegionNames.ProfileRegion, typeof(ProfileMainView));
            manager.RegisterViewWithRegion(BaseMethods.RegionNames.ScheduleRegion, typeof(ScheduleMainView));
            manager.RegisterViewWithRegion(BaseMethods.RegionNames.SettingsRegion, typeof(SettingsMainView));
        }
        private void OnLoaded()
        {
            manager.RequestNavigate(BaseMethods.RegionNames.LearnRegion, nameof(LearnMainView));
            manager.RequestNavigate(BaseMethods.RegionNames.ProfileRegion, nameof(ProfileMainView));
            manager.RequestNavigate(BaseMethods.RegionNames.ScheduleRegion, nameof(ScheduleMainView));
            manager.RequestNavigate(BaseMethods.RegionNames.SettingsRegion, nameof(SettingsMainView));
        }
        #endregion
    }
}
