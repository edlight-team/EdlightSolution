using ApplicationEventsWPF.Events;
using ApplicationModels;
using ApplicationModels.Models;
using ApplicationServices.WebApiService;
using ApplicationWPFServices.NotificationService;
using EdlightDesktopClient.Views.Dictionaries;
using HandyControl.Controls;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Styles.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace EdlightDesktopClient.ViewModels.Dictionaries
{
    public class DictionariesMainViewModel : BindableBase
    {
        #region services

        private readonly IRegionManager manager;
        private readonly IWebApiService api;
        private readonly INotificationService notification;

        #endregion
        #region fields

        private LoaderModel _loader;

        private ObservableCollection<UserModel> _teachers;
        private ObservableCollection<AcademicDisciplinesModel> _disciplines;
        private ObservableCollection<AudiencesModel> _audiences;
        private ObservableCollection<LearnPlanesModel> _planes;

        #endregion
        #region props

        public LoaderModel Loader { get => _loader; set => SetProperty(ref _loader, value); }

        public ObservableCollection<UserModel> Teachers { get => _teachers ??= new(); set => SetProperty(ref _teachers, value); }
        public ObservableCollection<AcademicDisciplinesModel> Disciplines { get => _disciplines ??= new(); set => SetProperty(ref _disciplines, value); }
        public ObservableCollection<AudiencesModel> Audiences { get => _audiences ??= new(); set => SetProperty(ref _audiences, value); }
        public ObservableCollection<LearnPlanesModel> Planes { get => _planes ??= new(); set => SetProperty(ref _planes, value); }

        #endregion
        #region commands

        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand AddDisciplineCommand { get; private set; }
        public DelegateCommand AddAudienceCommand { get; private set; }
        public DelegateCommand AddTeacherCommand { get; private set; }
        public DelegateCommand AddLearnPlanCommand { get; private set; }

        #endregion
        #region ctor and loading

        public DictionariesMainViewModel(IRegionManager manager, IWebApiService api, INotificationService notification, IEventAggregator aggregator)
        {
            Loader = new();
            this.manager = manager;
            this.api = api;
            this.notification = notification;

            aggregator.GetEvent<DictionaryModelChangedEvent>().Subscribe(OnDictionaryChanged);
            LoadedCommand = new DelegateCommand(OnLoaded);
            AddDisciplineCommand = new DelegateCommand(OnAddDiscipline);
            AddAudienceCommand = new DelegateCommand(OnAddAudience);
            AddTeacherCommand = new DelegateCommand(OnAddTeacher);
            AddLearnPlanCommand = new DelegateCommand(OnAddLearnPlan);
        }
        private async void OnLoaded()
        {
            Loader.SetDefaultLoadingInfo();
            Disciplines = new ObservableCollection<AcademicDisciplinesModel>(await api.GetModels<AcademicDisciplinesModel>(WebApiTableNames.AcademicDisciplines));
            Audiences = new ObservableCollection<AudiencesModel>(await api.GetModels<AudiencesModel>(WebApiTableNames.Audiences));

            foreach (AcademicDisciplinesModel discipline in Disciplines)
            {
                discipline.EditCommand = new DelegateCommand<object>(OnEditDiscipline);
                discipline.DeleteCommand = new DelegateCommand<object>(OnDeleteDiscipline);
            }
            foreach (AudiencesModel audience in Audiences)
            {
                audience.EditCommand = new DelegateCommand<object>(OnEditAudience);
                audience.DeleteCommand = new DelegateCommand<object>(OnDeleteAudience);
            }

            Teachers.Clear();
            RolesModel teach_role = (await api.GetModels<RolesModel>(WebApiTableNames.Roles, "RoleName = 'teacher'")).FirstOrDefault();
            List<UsersRolesModel> usersRoles = await api.GetModels<UsersRolesModel>(WebApiTableNames.UsersRoles, $"IdRole = '{teach_role.Id}'");
            List<UserModel> users = await api.GetModels<UserModel>(WebApiTableNames.Users);
            foreach (var item in usersRoles)
            {
                var user = users.FirstOrDefault(u => u.ID == item.IdUser);
                user.EditCommand = new DelegateCommand<object>(OnEditTeacher);
                user.DeleteCommand = new DelegateCommand<object>(OnDeleteTeacher);
                Teachers.Add(user);
            }

            Planes.Clear();
            List<LearnPlanesModel> planes = await api.GetModels<LearnPlanesModel>(WebApiTableNames.LearnPlanes);
            foreach (var item in planes)
            {
                item.OpenPlanCommand = new DelegateCommand<object>(OnOpenLearnPlan);
                item.DeletePlanCommand = new DelegateCommand<object>(OnDeleteLearnPlan);
                Planes.Add(item);
            }

            await Loader.Clear();
        }

        #endregion
        #region methods

        private void OnDictionaryChanged(object record)
        {
            if (record is AcademicDisciplinesModel discipline)
            {
                discipline.EditCommand = new DelegateCommand<object>(OnEditDiscipline);
                discipline.DeleteCommand = new DelegateCommand<object>(OnDeleteDiscipline);
                Disciplines.Add(discipline);
            }
            else if (record is LearnPlanesModel learnPlan)
            {
                learnPlan.OpenPlanCommand = new DelegateCommand<object>(OnOpenLearnPlan);
                learnPlan.DeletePlanCommand = new DelegateCommand<object>(OnDeleteLearnPlan);
                Planes.Add(learnPlan);
            }
            else if (record is AudiencesModel audience)
            {
                audience.EditCommand = new DelegateCommand<object>(OnEditAudience);
                audience.DeleteCommand = new DelegateCommand<object>(OnDeleteAudience);
                Audiences.Add(audience);
            }
            else if (record is UserModel teacher)
            {
                UserModel current = Teachers.FirstOrDefault(t => t.ID == teacher.ID);
                if (current != null)
                {
                    Teachers.Remove(current);
                }
                teacher.EditCommand = new DelegateCommand<object>(OnEditTeacher);
                teacher.DeleteCommand = new DelegateCommand<object>(OnDeleteTeacher);
                Teachers.Add(teacher);
            }
        }

        private void OnAddDiscipline() => manager.RequestNavigate(BaseMethods.RegionNames.ModalRegion, nameof(EditDisciplinesView),
                new NavigationParameters()
                {
                    { nameof(Audiences), Audiences }
                });
        private void OnEditDiscipline(object disciplineModel)
        {
            if (disciplineModel is AcademicDisciplinesModel model)
            {
                manager.RequestNavigate(BaseMethods.RegionNames.ModalRegion, nameof(EditDisciplinesView),
                    new NavigationParameters()
                    {
                        { nameof(Audiences), Audiences },
                        { "Model", model }
                    });
            }
        }
        private async void OnDeleteDiscipline(object disciplineModel)
        {
            bool? confirm = notification.ShowQuestion("Восстановить дисциплину невозможно, продолжить действие?");
            if (!confirm.HasValue || !confirm.Value) return;
            try
            {
                Loader.SetDefaultLoadingInfo();
                if (disciplineModel is AcademicDisciplinesModel model)
                {
                    await api.DeleteModel(model.Id, WebApiTableNames.AcademicDisciplines);
                    Disciplines.Remove(model);
                    Growl.Info("Дисциплина успешно удалена", "Global");
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

        private void OnAddAudience() => manager.RequestNavigate(BaseMethods.RegionNames.ModalRegion, nameof(EditAudienceView));
        private void OnEditAudience(object audienceModel)
        {
            if (audienceModel is AudiencesModel model)
            {
                manager.RequestNavigate(BaseMethods.RegionNames.ModalRegion, nameof(EditAudienceView), new NavigationParameters() { { "Model", model } });
            }
        }
        private async void OnDeleteAudience(object audienceModel)
        {
            bool? confirm = notification.ShowQuestion("Восстановить аудиторию невозможно, продолжить действие?");
            if (!confirm.HasValue || !confirm.Value) return;
            try
            {
                Loader.SetDefaultLoadingInfo();
                if (audienceModel is AudiencesModel model)
                {
                    await api.DeleteModel(model.Id, WebApiTableNames.Audiences);
                    Audiences.Remove(model);
                    Growl.Info("Аудитория успешно удалена", "Global");
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

        private void OnAddTeacher() => manager.RequestNavigate(BaseMethods.RegionNames.ModalRegion, nameof(EditTeacherView));
        private void OnEditTeacher(object userModel)
        {
            if (userModel is UserModel model)
            {
                manager.RequestNavigate(BaseMethods.RegionNames.ModalRegion, nameof(EditTeacherView), new NavigationParameters() { { "Model", model } });
            }
        }
        private async void OnDeleteTeacher(object userModel)
        {
            bool? confirm = notification.ShowQuestion("Восстановить пользователя невозможно, продолжить действие?");
            if (!confirm.HasValue || !confirm.Value) return;
            try
            {
                Loader.SetDefaultLoadingInfo();
                if (userModel is UserModel model)
                {
                    await api.DeleteModel(model.ID, WebApiTableNames.Users);
                    UsersRolesModel userRole = (await api.GetModels<UsersRolesModel>(WebApiTableNames.UsersRoles, $"IdUser = '{model.ID}'")).FirstOrDefault();
                    await api.DeleteModel(userRole.Id, WebApiTableNames.UsersRoles);
                    Teachers.Remove(model);
                    Growl.Info("Пользователь успешно удален", "Global");
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

        private void OnAddLearnPlan() => manager.RequestNavigate(BaseMethods.RegionNames.ModalRegion, nameof(EditLearnPlanView));
        private async void OnOpenLearnPlan(object planModel)
        {
            if (planModel is LearnPlanesModel model)
            {
                var file = await api.GetLearnPlan(model.Path);
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + model.Name + ".xlsx";
                if (file is JsonFileModel jfm)
                {
                    System.IO.File.WriteAllBytes(path, jfm.Data);
                    Process.Start(path);
                }
            }
        }
        private async void OnDeleteLearnPlan(object planModel)
        {
            bool? confirm = notification.ShowQuestion("Восстановить учебный план невозможно, продолжить действие?");
            if (!confirm.HasValue || !confirm.Value) return;
            try
            {
                Loader.SetDefaultLoadingInfo();
                if (planModel is LearnPlanesModel model)
                {
                    await api.DeleteLearnPlan(model.Path);
                    await api.DeleteModel(model.Id, WebApiTableNames.LearnPlanes);
                    Planes.Remove(model);
                    Growl.Info("Учебный план успешно удален", "Global");
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

        #endregion
    }
}
