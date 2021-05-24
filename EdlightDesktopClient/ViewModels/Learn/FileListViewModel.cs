using ApplicationEventsWPF.Events;
using ApplicationModels;
using ApplicationModels.Models;
using ApplicationServices.PermissionService;
using ApplicationServices.WebApiService;
using ApplicationWPFServices.MemoryService;
using ApplicationWPFServices.NotificationService;
using EdlightDesktopClient.AccessConfigurations;
using EdlightDesktopClient.Views.Learn;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Styles.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;

namespace EdlightDesktopClient.ViewModels.Learn
{
    public class FileListViewModel : BindableBase
    {
        #region services
        private IRegionManager manager;
        private IMemoryService memory;
        private IWebApiService api;
        private INotificationService notification;
        private IPermissionService permission;
        private IEventAggregator aggregator;
        #endregion
        #region fields
        private LoaderModel loader;
        private TestConfig config;
        private UserModel currentUser;

        private ObservableCollection<GroupsModel> groups;
        private ObservableCollection<ManualsFilesModel> manualFiles;
        private CollectionViewSource filteredStorageCards;
        private GroupsModel selectedGroup;

        private readonly GroupsModel allStorage;
        #endregion
        #region props
        public LoaderModel Loader { get => loader; set => SetProperty(ref loader, value); }
        public TestConfig Config { get => config; set => SetProperty(ref config, value); }

        public ObservableCollection<GroupsModel> Groups { get => groups; set => SetProperty(ref groups, value); }
        public ObservableCollection<ManualsFilesModel> ManualFiles { get => manualFiles ??= new(); set => SetProperty(ref manualFiles, value); }
        public CollectionViewSource FilteredManualFiles { get => filteredStorageCards ??= new(); set => SetProperty(ref filteredStorageCards, value); }
        public GroupsModel SelectedGroup
        {
            get => selectedGroup ??= new();
            set
            {
                SetProperty(ref selectedGroup, value);
                FilteredManualFiles.View?.Refresh();
            }
        }
        #endregion
        #region command
        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand<object> FileClickCommand { get; private set; }
        public DelegateCommand AddFileCommand { get; private set; }
        public DelegateCommand<object> DeleteFileCommand { get; private set; }
        public DelegateCommand<object> DownloadFileCommand { get; private set; }
        #endregion
        #region constructor
        public FileListViewModel(IRegionManager manager, IMemoryService memory, IWebApiService api, INotificationService notification, IPermissionService permission, IEventAggregator aggregator)
        {
            this.manager = manager;
            this.memory = memory;
            this.api = api;
            this.notification = notification;
            this.permission = permission;
            this.aggregator = aggregator;

            allStorage = new() { Group = "Все группы" };

            LoadedCommand = new(OnLoaded);

            AddFileCommand = new(OnAddFile);
            DeleteFileCommand = new(OnDeleteFile);
            DownloadFileCommand = new(OnDownloadFile);

            aggregator.GetEvent<ManualsFileAddedEvent>().Subscribe(OnLoaded);
        }
        #endregion
        #region methods
        #region loaded
        private async void OnLoaded()
        {
            try
            {
                Loader = new();

                currentUser = memory.GetItem<UserModel>(MemoryAlliases.CurrentUser);
                await permission.ConfigureService(api, currentUser);
                RolesModel student_role = await permission.GetRoleByName("student");
                RolesModel teacher_role = await permission.GetRoleByName("teacher");

                Task loading_task = Task.Run(async () => await LoadingData());
                await Task.WhenAll(loading_task);

                FilteredManualFiles.Source = ManualFiles;

                if (await permission.IsInRole(student_role))
                {
                    FilteredManualFiles.Filter += CardFilterStudent;
                    List<StudentsGroupsModel> studentsGroup = await api.GetModels<StudentsGroupsModel>(WebApiTableNames.StudentsGroups, $"IdStudent = '{currentUser.ID}'");
                    SelectedGroup = Groups.FirstOrDefault(g => g.Id == studentsGroup[0].IdGroup);
                }
                if (await permission.IsInRole(teacher_role))
                {
                    FilteredManualFiles.Filter += CardsFilter;
                    SelectedGroup = allStorage;
                }
            }
            catch (Exception ex)
            {
                notification.ShowError("Во время загрузки произошла ошибка: " + ex.Message);
                throw;
            }
            finally
            {
                Loader = new();
            }
        }
        #endregion
        private void CardsFilter(object sender, FilterEventArgs e)
        {
            if (SelectedGroup != null)
            {
                ManualsFilesModel file = e.Item as ManualsFilesModel;
                if (!Equals(SelectedGroup, allStorage))
                {
                    if (file.GroupID.ToString().ToLower() != selectedGroup.Id.ToString().ToLower())
                    {
                        e.Accepted = false;
                    }
                }
                if (file.CreatorID != currentUser.ID)
                {
                    e.Accepted = false;
                }
            }
        }

        private void CardFilterStudent(object sender, FilterEventArgs e)
        {
            if (SelectedGroup != null)
            {
                ManualsFilesModel file = e.Item as ManualsFilesModel;
                if (file.GroupID.ToString().ToLower() != selectedGroup.Id.ToString().ToLower())
                {
                    e.Accepted = false;
                }
            }
        }

        private async Task LoadingData(UserModel user = null)
        {
            if (Config == null) Config = await TestConfig.InitializeByPermissionService(permission);

            Groups = new();
            List<GroupsModel> gr = await api.GetModels<GroupsModel>(WebApiTableNames.Groups);
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                Groups.Add(allStorage);
                foreach (var item in gr)
                    Groups.Add(item);
            });
            ManualFiles = new ObservableCollection<ManualsFilesModel>(await api.GetModels<ManualsFilesModel>(WebApiTableNames.ManualsFiles));
            foreach (var item in ManualFiles)
                item.GroupName = Groups.Where(g => g.Id == item.GroupID).FirstOrDefault().Group;
        }
        #region command methods

        private async void OnDeleteFile(object parameter)
        {
            if (parameter is ManualsFilesModel manualsFile)
            {
                try
                {
                    await api.DeleteModel(manualsFile.ID, WebApiTableNames.ManualsFiles);

                    await api.DeleteFile($"Manuals\\{manualsFile.GroupID}\\{manualsFile.ID}");
                }
                catch (Exception ex)
                {
                    notification.ShowGlobalError("Во время загрузки произошла ошибка: " + ex.Message);
                    throw;
                }
                finally
                {
                    ManualFiles.Remove(manualsFile);
                    notification.ShowGlobalInformation("Файл удалён");
                    FilteredManualFiles.View?.Refresh();
                }
            }
        }

        private void OnAddFile() => manager.RequestNavigate(BaseMethods.RegionNames.ModalRegion, nameof(AddManualFIleView));

        private async void OnDownloadFile(object parameter)
        {
            if (parameter is ManualsFilesModel manualsFile)
            {
                try
                {
                    FolderBrowserDialog folderFileDialog = new();
                    if (folderFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string pathFile = folderFileDialog.SelectedPath;

                        JsonFileModel file = (JsonFileModel)await api.GetFile($"Manuals\\{manualsFile.GroupID}\\{manualsFile.ID}\\{manualsFile.FileName}");

                        File.WriteAllBytes(pathFile + $"\\{manualsFile.FileName}", file.Data);
                    }
                }
                catch (Exception ex)
                {
                    notification.ShowGlobalError("Во время загрузки произошла ошибка: " + ex.Message);
                    throw;
                }
                finally
                {
                    notification.ShowGlobalInformation("Файл успешно загружен");
                }
            }
        }
        #endregion
        #endregion
    }
}
