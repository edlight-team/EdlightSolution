using ApplicationModels.Models;
using ApplicationServices.PermissionService;
using ApplicationServices.WebApiService;
using ApplicationWPFServices.MemoryService;
using ApplicationWPFServices.NotificationService;
using Prism.Commands;
using Prism.Mvvm;
using Styles.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace EdlightDesktopClient.ViewModels.Groups
{
    public class GroupsMainViewModel : BindableBase
    {
        #region services
        private readonly IMemoryService memory;
        private readonly IWebApiService api;
        private readonly INotificationService notification;
        private readonly IPermissionService permission;
        #endregion
        #region fields
        private LoaderModel loader;

        private ObservableCollection<GroupsModel> inputGroups;
        private ObservableCollection<GroupsModel> outputGroups;
        private GroupsModel inputSelectedGroup;
        private GroupsModel outputSelectedGroup;

        private readonly GroupsModel withoutGroup;

        private List<StudentsGroupsModel> studentsGroups;
        private List<UserModel> students;
        private ObservableCollection<UserModel> selctedGroupStudents;
        #endregion
        #region props
        public LoaderModel Loader { get => loader; set => SetProperty(ref loader, value); }

        public ObservableCollection<GroupsModel> InputGroups { get => inputGroups ??= new(); set => SetProperty(ref inputGroups, value); }
        public ObservableCollection<GroupsModel> OutputGroups { get => outputGroups ??= new(); set => SetProperty(ref outputGroups, value); }
        public GroupsModel InputSelectedGroup
        {
            get => inputSelectedGroup ??= new();
            set
            {
                if (SetProperty(ref inputSelectedGroup, value))
                    RefreshSelectedGroupStudent(value);
            }
        }
        public GroupsModel OutputSelectedGroup { get => outputSelectedGroup ??= new(); set => SetProperty(ref outputSelectedGroup, value); }

        public List<UserModel> Students { get => students ??= new(); set => SetProperty(ref students, value); }
        public ObservableCollection<UserModel> SelectedGroupStudents { get => selctedGroupStudents ??= new(); set => SetProperty(ref selctedGroupStudents, value); }
        #endregion
        #region command
        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand<object> SaveGroupsChangedCommand { get; private set; }
        #endregion
        #region constructor
        public GroupsMainViewModel(IMemoryService memory, IWebApiService api, INotificationService notification, IPermissionService permission)
        {
            Loader = new();
            this.memory = memory;
            this.api = api;
            this.notification = notification;
            this.permission = permission;

            LoadedCommand = new(OnLoaded);
            SaveGroupsChangedCommand = new(OnSaveGroupsChanged);

            withoutGroup = new() { Group = "Без группы" };
        }
        #endregion
        #region methods
        private async void OnLoaded()
        {
            try
            {
                Loader.SetDefaultLoadingInfo();

                Task loading_task = Task.Run(async () => await LoadingData());
                await Task.WhenAll(loading_task);
                
            }
            catch (Exception ex)
            {
                notification.ShowError("Во время загрузки произошла ошибка: " + ex.Message);
            }
            finally
            {
                await Loader.Clear();
            }
        }

        private void RefreshSelectedGroupStudent(GroupsModel selectedGroup)
        {
            if (selectedGroup != null)
            {
                SelectedGroupStudents = new();
                if (Equals(InputSelectedGroup, withoutGroup))
                {
                    foreach (var item in Students)
                    {
                        if (!studentsGroups.Any(s => s.IdStudent == item.ID))
                        {
                            SelectedGroupStudents.Add(item);
                        }
                    }
                }
                else
                {
                    foreach (var item in Students)
                    {
                        if (studentsGroups.Any(s => s.IdStudent == item.ID && s.IdGroup == InputSelectedGroup.Id))
                        {
                            SelectedGroupStudents.Add(item);
                        }
                    }
                }
            }
        }

        private async Task LoadingData(UserModel user = null)
        {
            InputGroups = new();
            List<GroupsModel> gr = await api.GetModels<GroupsModel>(WebApiTableNames.Groups);
            Application.Current.Dispatcher.Invoke(() =>
            {
                InputGroups.Add(withoutGroup);
                foreach (var item in gr)
                    InputGroups.Add(item);
            });
            Application.Current.Dispatcher.Invoke(() =>
            {
                OutputGroups.Add(withoutGroup);
                foreach (var item in gr)
                    OutputGroups.Add(item);
            });

            RolesModel student_role = await permission.GetRoleByName("student");
            List<UsersRolesModel> usersRoles = await api.GetModels<UsersRolesModel>(WebApiTableNames.UsersRoles, $"IdRole = '{student_role.Id}'");
            List<UserModel> allUsers = await api.GetModels<UserModel>(WebApiTableNames.Users);

            studentsGroups = await api.GetModels<StudentsGroupsModel>(WebApiTableNames.StudentsGroups);

            foreach (var item in usersRoles)
                Students.Add(allUsers.Where(u => u.ID == item.IdUser).FirstOrDefault());
        }

        private async void OnSaveGroupsChanged(object parameter)
        {
            if (OutputSelectedGroup == default)
            {
                notification.ShowInformation("Выбирете группу в которую будут пермещены суденты");
                return;
            }

            var objectsCollection = ((ObservableCollection<object>)parameter).ToList();
            var models = objectsCollection.Cast<UserModel>().ToList();

            if (models != null)
            {
                try
                {
                    if (Equals(OutputSelectedGroup, withoutGroup))
                    {
                        foreach (var item in models)
                        {
                            StudentsGroupsModel deletedStudentGroup = studentsGroups.Where(s => s.IdStudent == item.ID).FirstOrDefault();
                            await api.DeleteModel(deletedStudentGroup.Id, WebApiTableNames.StudentsGroups);
                            studentsGroups.Remove(deletedStudentGroup);
                        }
                    }
                    else
                    {
                        foreach (var item in models)
                        {
                            StudentsGroupsModel updatedStudentGroup = studentsGroups.Where(s => s.IdStudent == item.ID).FirstOrDefault();
                            updatedStudentGroup.IdGroup = OutputSelectedGroup.Id;
                            await api.PutModel(updatedStudentGroup, WebApiTableNames.StudentsGroups);
                        }
                    }
                }
                catch (Exception ex)
                {
                    notification.ShowGlobalError(ex.Message);
                }
                finally
                {
                    RefreshSelectedGroupStudent(InputSelectedGroup);
                }
            }
        }
        #endregion
    }
}
