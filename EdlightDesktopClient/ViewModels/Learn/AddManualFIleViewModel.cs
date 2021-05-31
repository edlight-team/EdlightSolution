using ApplicationEventsWPF.Events.LearnEvents;
using ApplicationModels;
using ApplicationModels.Models;
using ApplicationServices.WebApiService;
using ApplicationWPFServices.MemoryService;
using ApplicationWPFServices.NotificationService;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Styles.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace EdlightDesktopClient.ViewModels.Learn
{
    public class AddManualFIleViewModel : BindableBase
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
        private string fileName;

        private byte[] file;
        #endregion
        #region props
        public LoaderModel Loader { get => loader; set => SetProperty(ref loader, value); }

        public List<GroupsModel> Groups { get => groups; set => SetProperty(ref groups, value); }
        public GroupsModel SelectedGroup { get => selectedGroup; set => SetProperty(ref selectedGroup, value); }
        public string FileName { get => fileName; set => SetProperty(ref fileName, value); }
        #endregion
        #region command
        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand AddFileCommand { get; private set; }
        public DelegateCommand SaveStorageCommand { get; private set; }
        public DelegateCommand CloseModalCommand { get; private set; }
        #endregion
        #region constructor
        public AddManualFIleViewModel(IWebApiService api, INotificationService notification, IRegionManager manager, IMemoryService memory, IEventAggregator aggregator)
        {
            this.api = api;
            this.notification = notification;
            this.manager = manager;
            this.memory = memory;
            this.aggregator = aggregator;

            LoadedCommand = new(OnLoaded);
            AddFileCommand = new(OnAddFile);
            SaveStorageCommand = new(OnSaveFile);
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

        private async void OnSaveFile()
        {
            if (string.IsNullOrEmpty(FileName))
            {
                notification.ShowInformation("Выберите файл");
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

                ManualsFilesModel manualsFile = new();
                manualsFile.FileName = this.FileName;
                manualsFile.CreatorID = currentUser.ID;
                manualsFile.GroupID = SelectedGroup.Id;

                manualsFile = await api.PostModel(manualsFile, WebApiTableNames.ManualsFiles);

                JsonFileModel fileModel = new()
                {
                    FileName = this.FileName,
                    Data = file
                };

                await api.PushFile($"Manuals\\{manualsFile.GroupID}\\{manualsFile.ID}\\", fileModel);
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
                aggregator.GetEvent<ManualsFileAddedEvent>().Publish();
            }
        }

        private void OnAddFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                FileName = openFileDialog.SafeFileName;

                file = File.ReadAllBytes(filePath);
            }
        }

        private void OnCloseCommand() => manager.Regions[BaseMethods.RegionNames.ModalRegion].RemoveAll();
        #endregion
    }
}
