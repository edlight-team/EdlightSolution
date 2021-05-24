using ApplicationEventsWPF.Events;
using ApplicationModels.Models;
using ApplicationServices.WebApiService;
using ApplicationWPFServices.MemoryService;
using ApplicationWPFServices.NotificationService;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Styles.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdlightDesktopClient.ViewModels.Learn
{
    public class AddStorageViewModel : BindableBase
    {
        #region services
        private IWebApiService api;
        private INotificationService notification;
        private IRegionManager manager;
        private IMemoryService memory;
        private IEventAggregator aggregator;
        #endregion
        #region fields
        private LoaderModel loader;

        private List<GroupsModel> groups;
        private GroupsModel selectedGroup;
        private string storageName;
        private DateTime endStorageDateTime;
        #endregion
        #region props
        public LoaderModel Loader { get => loader; set => SetProperty(ref loader, value); }

        public List<GroupsModel> Groups { get => groups; set => SetProperty(ref groups, value); }
        public GroupsModel SelectedGroup { get => selectedGroup; set => SetProperty(ref selectedGroup, value); }
        public string StorageName { get => storageName; set => SetProperty(ref storageName, value); }
        public DateTime EndStorageDateTime { get => endStorageDateTime; set => SetProperty(ref endStorageDateTime, value); }
        #endregion
        #region command
        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand SaveStorageCommand { get; private set; }
        public DelegateCommand CloseModalCommand { get; private set; }
        #endregion
        #region constructor
        public AddStorageViewModel(IWebApiService api, INotificationService notification, IRegionManager manager, IMemoryService memory, IEventAggregator aggregator)
        {
            this.api = api;
            this.notification = notification;
            this.manager = manager;
            this.memory = memory;
            this.aggregator = aggregator;

            LoadedCommand = new(OnLoaded);
            SaveStorageCommand = new(OnSaveStorage);
            CloseModalCommand = new(OnCloseCommand);
        }
        #endregion
        #region methods
        private async void OnLoaded()
        {
            try
            {
                Loader = new();

                Groups = await api.GetModels<GroupsModel>(WebApiTableNames.Groups);
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

        private async void OnSaveStorage()
        {
            if (string.IsNullOrEmpty(StorageName))
            {
                notification.ShowInformation("Введите название хранилища");
                return;
            }
            if (SelectedGroup == null)
            {
                notification.ShowInformation("Выберите группу");
                return;
            }

            try
            {
                Loader = new();

                UserModel currentUser = memory.GetItem<UserModel>(MemoryAlliases.CurrentUser);

                StoragesHeadersModel storage = new();
                storage.StorageName = StorageName;
                storage.CreatorID = currentUser.ID;
                storage.GroupID = SelectedGroup.Id;
                storage.DateCloseStorage = EndStorageDateTime.ToString("G");

                storage = await api.PostModel(storage, WebApiTableNames.StoragesHeaders);

                List<StudentsGroupsModel> studentsGroups = await api.GetModels<StudentsGroupsModel>(WebApiTableNames.StudentsGroups, $"IdGroup = '{storage.GroupID}'");
                List<UserModel> students = new();
                foreach (var item in studentsGroups)
                    students.Add((await api.GetModels<UserModel>(WebApiTableNames.Users, $"ID = '{item.IdStudent}'")).FirstOrDefault());

                foreach (var item in students)
                {
                    await api.PostModel(new StorageFilesModel()
                    {
                        StorageID = storage.ID,
                        StudentID = item.ID,
                        FileName = string.Empty
                    }, WebApiTableNames.StorageFiles);
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
                manager.Regions[BaseMethods.RegionNames.ModalRegion].RemoveAll();
                aggregator.GetEvent<StorageCollectionUpdatedEvent>().Publish();
            }
        }

        private void OnCloseCommand() => manager.Regions[BaseMethods.RegionNames.ModalRegion].RemoveAll();
        #endregion
    }
}
