using ApplicationModels;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Linq;
using Styles.Models;
using System.Collections.ObjectModel;
using ApplicationModels.Models.CapacityExtendedModels;
using ApplicationModels.Models;
using System.Collections.Generic;
using ApplicationServices.WebApiService;

namespace EdlightDesktopClient.ViewModels.Schedule
{
    [RegionMemberLifetime(KeepAlive = false)]
    public class CapacityManagmentViewModel : BindableBase, INavigationAware
    {
        #region services

        private readonly IRegionManager manager;
        private readonly IWebApiService api;

        #endregion
        #region fields

        private LoaderModel _loader;
        private ObservableCollection<CapacityModel> _capacities;

        private ObservableCollection<UserModel> _teachers;
        private ObservableCollection<ImportedTeacher> _teacher_FIO;

        #endregion
        #region props

        public LoaderModel Loader { get => _loader; set => SetProperty(ref _loader, value); }
        public ObservableCollection<CapacityModel> Capacities { get => _capacities ??= new(); set => SetProperty(ref _capacities, value); }

        public ObservableCollection<UserModel> Teachers { get => _teachers ??= new(); set => SetProperty(ref _teachers, value); }
        public ObservableCollection<ImportedTeacher> Teacher_FIO { get => _teacher_FIO ??= new(); set => SetProperty(ref _teacher_FIO, value); }

        #endregion
        #region commands

        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand CloseModalCommand { get; private set; }

        #endregion
        #region ctor

        public CapacityManagmentViewModel(IRegionManager manager, IWebApiService api)
        {
            this.manager = manager;
            this.api = api;

            LoadedCommand = new DelegateCommand(OnLoaded);
            CloseModalCommand = new DelegateCommand(OnCloseModal);
        }

        #endregion
        #region methods

        private async void OnLoaded()
        {
            Teachers.Clear();
            RolesModel teach_role = (await api.GetModels<RolesModel>(WebApiTableNames.Roles, "RoleName = 'teacher'")).FirstOrDefault();
            List<UsersRolesModel> usersRoles = await api.GetModels<UsersRolesModel>(WebApiTableNames.UsersRoles, $"IdRole = '{teach_role.Id}'");
            List<UserModel> users = await api.GetModels<UserModel>(WebApiTableNames.Users);
            foreach (var item in usersRoles)
            {
                var user = users.FirstOrDefault(u => u.ID == item.IdUser);
                Teachers.Add(user);
            }

            Teacher_FIO.Clear();
            var teacher_group = Capacities.GroupBy(g => g.TeacherFio);
            foreach (var item in teacher_group)
            {
                if (item.Key != string.Empty)
                {
                    //Проверяем есть ли в преподавтелях найденные из нагрузки
                    ImportedTeacher imported = new();
                    imported.TeacherInitials = item.Key.Remove(0, 1);

                    var equal_by_initials = Teachers.FirstOrDefault(t => t.Initials == imported.TeacherInitials);
                    if (equal_by_initials != null)
                    {
                        imported.IsLookUpOnDB = true;
                        imported.LookUpUser = equal_by_initials;
                    }
                    else
                    {
                        imported.IsLookUpOnDB = false;
                    }
                    imported.IsConfirmedOrSkipped = false;
                    imported.ConfirmCommand = new DelegateCommand<object>(OnConfirmImportModel);

                    Teacher_FIO.Add(imported);
                }
            }
        }
        private void OnCloseModal() => manager.Regions[BaseMethods.RegionNames.ModalRegion].RemoveAll();

        private void OnConfirmImportModel(object model)
        {
            if (model is ImportedTeacher it)
            {
                it.IsConfirmEnabled = true;
            }
        }

        #endregion
        #region navigation
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey(nameof(Capacities)) && navigationContext.Parameters[nameof(Capacities)] is ObservableCollection<CapacityModel> oc)
            {
                Capacities = oc;
            }
        }
        public void OnNavigatedFrom(NavigationContext navigationContext) { }
        public bool IsNavigationTarget(NavigationContext navigationContext) => true; 
        #endregion
    }
}
