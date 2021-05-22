using ApplicationModels.Models;
using ApplicationServices.PermissionService;
using ApplicationServices.WebApiService;
using ApplicationWPFServices.MemoryService;
using EdlightDesktopClient.AccessConfigurations;
using EdlightDesktopClient.BaseMethods;
using EdlightDesktopClient.Views.Dictionaries;
using EdlightDesktopClient.Views.Groups;
using EdlightDesktopClient.Views.Learn;
using EdlightDesktopClient.Views.Profile;
using EdlightDesktopClient.Views.Schedule;
using EdlightDesktopClient.Views.Settings;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Styles.Models;
using System.Windows;

namespace EdlightDesktopClient.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region services

        private readonly IRegionManager manager;
        private readonly IMemoryService memory;
        private readonly IWebApiService api;
        private readonly IPermissionService permissionService;

        #endregion
        #region fields

        private string _title = "Edlight";
        private WindowState _currentState;
        private LoaderModel _loader;
        private MainConfig _config;

        #endregion
        #region props

        public string Title { get => _title; set => SetProperty(ref _title, value); }
        public WindowState CurrentState { get => _currentState; set => SetProperty(ref _currentState, value); }
        public LoaderModel Loader { get => _loader; set => SetProperty(ref _loader, value); }
        public MainConfig Config { get => _config ??= new(); set => SetProperty(ref _config, value); }

        #endregion
        #region commands

        public DelegateCommand MinimizeCommand { get; private set; }
        public DelegateCommand CloseCommand { get; private set; }
        public DelegateCommand LoadedCommand { get; private set; }

        #endregion
        #region ctor

        public MainWindowViewModel(IRegionManager manager, IMemoryService memory, IWebApiService api, IPermissionService permissionService)
        {
            Loader = new();
            this.manager = manager;
            this.memory = memory;
            this.api = api;
            this.permissionService = permissionService;

            MinimizeCommand = new DelegateCommand(() => CurrentState = StaticCommands.ChangeWindowState(CurrentState));
            CloseCommand = new DelegateCommand(StaticCommands.Shutdown);
            LoadedCommand = new DelegateCommand(OnLoaded);
        }
        private async void OnLoaded()
        {
            await permissionService.ConfigureService(api, memory.GetItem<UserModel>(MemoryAlliases.CurrentUser));
            await Config.SetVisibilities(permissionService);

            manager.RequestNavigate(BaseMethods.RegionNames.LearnRegion, nameof(LearnMainView));
            manager.RequestNavigate(BaseMethods.RegionNames.ProfileRegion, nameof(ProfileMainView));
            manager.RequestNavigate(BaseMethods.RegionNames.SettingsRegion, nameof(SettingsMainView));
            manager.RequestNavigate(BaseMethods.RegionNames.GroupsRegion, nameof(GroupsMainView));
            manager.RequestNavigate(BaseMethods.RegionNames.ScheduleRegion, nameof(ScheduleMainView));
            manager.RequestNavigate(BaseMethods.RegionNames.ScheduleDateViewRegion, nameof(ScheduleDateViewer));
            manager.RequestNavigate(BaseMethods.RegionNames.DictionariesRegion, nameof(DictionariesMainView));
        }
        #endregion
    }
}
