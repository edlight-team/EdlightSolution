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
using Prism.Services.Dialogs;
using EdlightDesktopClient.Views.Schedule.CapacityWindows;
using System;

namespace EdlightDesktopClient.ViewModels.Schedule
{
    [RegionMemberLifetime(KeepAlive = false)]
    public class CapacityManagmentViewModel : BindableBase, INavigationAware
    {
        #region services

        private readonly IRegionManager manager;
        private readonly IWebApiService api;
        private readonly IDialogService dialog;

        #endregion
        #region fields

        private LoaderModel _loader;
        private ObservableCollection<CapacityModel> _capacities;

        private ObservableCollection<AudiencesModel> _audiences;
        private ObservableCollection<UserModel> _teachers;
        private ObservableCollection<AcademicDisciplinesModel> _disciplines;
        private ObservableCollection<ImportedTeacher> _importedTeachers;
        private ObservableCollection<ImportedDiscipline> _importedDisciplines;
        private bool _isAllTeachersConfirmed;
        private bool _isAllDisciplinesConfirmed;

        #endregion
        #region props

        public LoaderModel Loader { get => _loader; set => SetProperty(ref _loader, value); }
        public ObservableCollection<CapacityModel> Capacities { get => _capacities ??= new(); set => SetProperty(ref _capacities, value); }

        public ObservableCollection<AudiencesModel> Audiences { get => _audiences ??= new(); set => SetProperty(ref _audiences, value); }
        public ObservableCollection<UserModel> Teachers { get => _teachers ??= new(); set => SetProperty(ref _teachers, value); }
        public ObservableCollection<AcademicDisciplinesModel> Disciplines { get => _disciplines ??= new(); set => SetProperty(ref _disciplines, value); }
        public ObservableCollection<ImportedTeacher> ImportedTeachers { get => _importedTeachers ??= new(); set => SetProperty(ref _importedTeachers, value); }
        public ObservableCollection<ImportedDiscipline> ImportedDisciplines { get => _importedDisciplines ??= new(); set => SetProperty(ref _importedDisciplines, value); }
        public bool IsAllTeachersConfirmed { get => _isAllTeachersConfirmed; set => SetProperty(ref _isAllTeachersConfirmed, value); }
        public bool IsAllDisciplinesConfirmed { get => _isAllDisciplinesConfirmed; set => SetProperty(ref _isAllDisciplinesConfirmed, value); }

        #endregion
        #region commands

        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand CloseModalCommand { get; private set; }

        public DelegateCommand AcceptAllEqualsTeachersCommand { get; private set; }
        public DelegateCommand AcceptAllEqualsDisciplinesCommand { get; private set; }
        public DelegateCommand GenerateScheduleCommand { get; private set; }

        #endregion
        #region ctor

        public CapacityManagmentViewModel(IRegionManager manager, IWebApiService api, IDialogService dialog)
        {
            Loader = new();
            this.manager = manager;
            this.api = api;
            this.dialog = dialog;

            LoadedCommand = new DelegateCommand(OnLoaded);
            CloseModalCommand = new DelegateCommand(OnCloseModal);
            AcceptAllEqualsTeachersCommand = new DelegateCommand(OnAcceptAllEqualsTeachers);
            AcceptAllEqualsDisciplinesCommand = new DelegateCommand(OnAcceptAllEqualsDisciplines);
            GenerateScheduleCommand = new DelegateCommand(OnScheduleGenerating, () => IsAllTeachersConfirmed && IsAllDisciplinesConfirmed)
                .ObservesProperty(() => IsAllDisciplinesConfirmed)
                .ObservesProperty(() => IsAllTeachersConfirmed);
        }

        #endregion
        #region methods

        private async void OnLoaded()
        {
            try
            {
                Loader.SetDefaultLoadingInfo();
                Audiences.Clear();
                Audiences.AddRange(await api.GetModels<AudiencesModel>(WebApiTableNames.Audiences));

                Teachers.Clear();
                RolesModel teach_role = (await api.GetModels<RolesModel>(WebApiTableNames.Roles, "RoleName = 'teacher'")).FirstOrDefault();
                List<UsersRolesModel> usersRoles = await api.GetModels<UsersRolesModel>(WebApiTableNames.UsersRoles, $"IdRole = '{teach_role.Id}'");
                List<UserModel> users = await api.GetModels<UserModel>(WebApiTableNames.Users);
                foreach (var item in usersRoles)
                {
                    var user = users.FirstOrDefault(u => u.ID == item.IdUser);
                    Teachers.Add(user);
                }
                Disciplines.Clear();
                Disciplines.AddRange(await api.GetModels<AcademicDisciplinesModel>(WebApiTableNames.AcademicDisciplines));

                ImportedTeachers.Clear();
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

                        imported.StartConfirming = new DelegateCommand<object>(OnConfirmTeacherStarting);
                        imported.ConfirmCommand = new DelegateCommand<object>(OnConfirmImportTeacherModel);
                        imported.CreateTeacherCommand = new DelegateCommand<object>(OnCreateTeacher);

                        ImportedTeachers.Add(imported);
                    }
                }

                ImportedDisciplines.Clear();
                var disciplines_group = Capacities.GroupBy(g => g.DisciplineOrWorkType);
                List<string> disciplines_disctinct = new();
                foreach (var item in disciplines_group.ToList())
                {
                    if (!disciplines_disctinct.Any(dd => dd.StartsWith(item.Key.Trim())) && !item.Key.Trim().Contains("п/г"))
                    {
                        disciplines_disctinct.Add(item.Key);
                    }
                }
                foreach (var item in disciplines_disctinct)
                {
                    //Проверяем есть ли в преподавтелях найденные из нагрузки
                    ImportedDiscipline imported = new();
                    imported.DisciplineTitle = item;

                    var equal_by_initials = Disciplines.FirstOrDefault(t => t.Title == imported.DisciplineTitle);
                    if (equal_by_initials != null)
                    {
                        imported.IsLookUpOnDB = true;
                        imported.LookUpDiscipline = equal_by_initials;
                    }
                    else
                    {
                        imported.IsLookUpOnDB = false;
                    }
                    imported.IsConfirmedOrSkipped = false;

                    imported.StartConfirming = new DelegateCommand<object>(OnConfirmDisciplineStarting);
                    imported.ConfirmCommand = new DelegateCommand<object>(OnConfirmImportDisciplineModel);
                    imported.CreateDisciplineCommand = new DelegateCommand<object>(OnCreateDiscipline);

                    ImportedDisciplines.Add(imported);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await Loader.Clear();
            }
        }
        private void OnCloseModal() => manager.Regions[BaseMethods.RegionNames.ModalRegion].RemoveAll();

        #region Teachers

        private void OnConfirmTeacherStarting(object model)
        {
            if (model is ImportedTeacher it)
            {
                it.IsConfirmEnabled = true;
            }
        }
        private void OnConfirmImportTeacherModel(object model)
        {
            if (model is ImportedTeacher it)
            {
                it.IsConfirmedOrSkipped = true;
                IsAllTeachersConfirmed = ImportedTeachers.All(t => t.IsConfirmedOrSkipped);
            }
        }
        private void OnCreateTeacher(object model)
        {
            if (model is ImportedTeacher it)
            {
                dialog.ShowDialog(
                    nameof(CreateTeacherWindow),
                    new DialogParameters() { { nameof(ImportedTeacher), it } },
                    null
                    );
                IsAllTeachersConfirmed = ImportedTeachers.All(t => t.IsConfirmedOrSkipped);
            }
        }
        private void OnAcceptAllEqualsTeachers()
        {
            foreach (var item in ImportedTeachers)
            {
                if (item.IsLookUpOnDB)
                {
                    item.IsConfirmEnabled = true;
                    item.IsConfirmedOrSkipped = true;
                }
            }
            IsAllTeachersConfirmed = ImportedTeachers.All(t => t.IsConfirmedOrSkipped);
        }

        #endregion
        #region Disciplines

        private void OnConfirmDisciplineStarting(object model)
        {
            if (model is ImportedDiscipline it)
            {
                it.IsConfirmEnabled = true;
            }
        }
        private void OnConfirmImportDisciplineModel(object model)
        {
            if (model is ImportedDiscipline it)
            {
                it.IsConfirmedOrSkipped = true;
                IsAllDisciplinesConfirmed = ImportedDisciplines.All(t => t.IsConfirmedOrSkipped);
            }
        }
        private void OnCreateDiscipline(object model)
        {
            if (model is ImportedDiscipline it)
            {
                dialog.ShowDialog(
                    nameof(CreateDisciplineWindow),
                    new DialogParameters() { { nameof(ImportedDiscipline), it }, { nameof(Audiences), Audiences } },
                    null
                    );
                IsAllDisciplinesConfirmed = ImportedDisciplines.All(t => t.IsConfirmedOrSkipped);
            }
        }
        private void OnAcceptAllEqualsDisciplines()
        {
            foreach (var item in ImportedDisciplines)
            {
                if (item.IsLookUpOnDB)
                {
                    item.IsConfirmEnabled = true;
                    item.IsConfirmedOrSkipped = true;
                }
            }
            IsAllDisciplinesConfirmed = ImportedDisciplines.All(t => t.IsConfirmedOrSkipped);
        }

        #endregion
        #region Schedule

        private void OnScheduleGenerating()
        {
            var date_froms = Capacities.GroupBy(g => g.DateFrom);
            var date_toes = Capacities.GroupBy(g => g.DateTo);
        }

        #endregion

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
