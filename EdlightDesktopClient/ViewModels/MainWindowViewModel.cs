using ApplicationModels.Models;
using ApplicationServices.PermissionService;
using ApplicationServices.WebApiService;
using ApplicationWPFServices.MemoryService;
using EdlightDesktopClient.BaseMethods;
using EdlightDesktopClient.Views.Groups;
using EdlightDesktopClient.Views.Learn;
using EdlightDesktopClient.Views.Profile;
using EdlightDesktopClient.Views.Schedule;
using EdlightDesktopClient.Views.Settings;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Styles.Models;
using System.Threading.Tasks;
using System.Windows;

namespace EdlightDesktopClient.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region services

        private readonly IRegionManager manager;
        private readonly IMemoryService memory;
        private readonly IWebApiService api;
        private readonly IPermissionService accessManager;

        #endregion
        #region fields

        private string _title = "Edlight";
        private WindowState _currentState;
        private LoaderModel _loader;
        private Visibility _groupsVisibility;

        #endregion
        #region props

        public string Title { get => _title; set => SetProperty(ref _title, value); }
        public WindowState CurrentState { get => _currentState; set => SetProperty(ref _currentState, value); }
        public LoaderModel Loader { get => _loader; set => SetProperty(ref _loader, value); }
        public Visibility GroupsVisibility { get => _groupsVisibility; set => SetProperty(ref _groupsVisibility, value); }

        #endregion
        #region commands

        public DelegateCommand MinimizeCommand { get; private set; }
        public DelegateCommand CloseCommand { get; private set; }
        public DelegateCommand LoadedCommand { get; private set; }

        #endregion
        #region ctor

        public MainWindowViewModel(IRegionManager manager, IMemoryService memory, IWebApiService api, IPermissionService accessManager)
        {
            Loader = new();
            GroupsVisibility = Visibility.Collapsed;
            this.manager = manager;
            this.memory = memory;
            this.api = api;
            this.accessManager = accessManager;

            MinimizeCommand = new DelegateCommand(() => CurrentState = StaticCommands.ChangeWindowState(CurrentState));
            CloseCommand = new DelegateCommand(StaticCommands.Shutdown);
            LoadedCommand = new DelegateCommand(OnLoaded);

            Task.Run(async () =>
            {
                //Проверяем пользователя
                UserModel current_user = memory.GetItem<UserModel>(MemoryAlliases.CurrentUser);
                await accessManager.ConfigureService(api, current_user);
                RolesModel teacher_role = await accessManager.GetRoleByName("teacher");
                if (await accessManager.IsInRole(teacher_role)) GroupsVisibility = Visibility.Visible;
            });
        }
        private void OnLoaded()
        {
            manager.RequestNavigate(BaseMethods.RegionNames.LearnRegion, nameof(LearnMainView));
            manager.RequestNavigate(BaseMethods.RegionNames.ProfileRegion, nameof(ProfileMainView));
            manager.RequestNavigate(BaseMethods.RegionNames.SettingsRegion, nameof(SettingsMainView));
            manager.RequestNavigate(BaseMethods.RegionNames.GroupsRegion, nameof(GroupsMainView));
            manager.RequestNavigate(BaseMethods.RegionNames.ScheduleRegion, nameof(ScheduleMainView));
            manager.RequestNavigate(BaseMethods.RegionNames.ScheduleDateViewRegion, nameof(ScheduleDateViewer));
        }
        #endregion
    }
}
