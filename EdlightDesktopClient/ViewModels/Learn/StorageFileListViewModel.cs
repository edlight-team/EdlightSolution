using ApplicationModels;
using ApplicationModels.Models;
using ApplicationServices.WebApiService;
using ApplicationWPFServices.NotificationService;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Styles.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EdlightDesktopClient.ViewModels.Learn
{
    public class StorageFileListViewModel : BindableBase, INavigationAware
    {
        #region services
        private IRegionManager manager;
        private IWebApiService api;
        private INotificationService notification;
        private IEventAggregator aggregator;
        #endregion
        #region fields
        private LoaderModel loader;

        private List<StorageFilesModel> storageFiles;
        private List<UserModel> studens;
        private StoragesHeadersModel currentStorage;
        #endregion
        #region props
        public LoaderModel Loader { get => loader; set => SetProperty(ref loader, value); }

        public List<StorageFilesModel> StorageFiles { get => storageFiles ??= new(); set => SetProperty(ref storageFiles, value); }
        public List<UserModel> Studens { get => studens ??= new(); set => SetProperty(ref studens, value); }
        public StoragesHeadersModel CurrentStorage { get => currentStorage ??= new(); set => SetProperty(ref currentStorage, value); }
        #endregion
        #region commands
        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand CloseModalCommand { get; private set; }
        public DelegateCommand<object> DownloadFileCommand { get; private set; }
        #endregion
        #region constructor
        public StorageFileListViewModel(IRegionManager manager, IWebApiService api, INotificationService notification, IEventAggregator aggregator)
        {
            this.manager = manager;
            this.api = api;
            this.notification = notification;
            this.aggregator = aggregator;

            LoadedCommand = new(OnLoaded);
            DownloadFileCommand = new(OnDownloadFile);
            CloseModalCommand = new(OnCloseModal);
        }
        #endregion
        #region methods
        private async void OnLoaded()
        {
            try
            {
                Loader = new();

                StorageFiles = await api.GetModels<StorageFilesModel>(WebApiTableNames.StorageFiles, $"StorageID = '{CurrentStorage.ID}'");

                foreach (var item in StorageFiles)
                    Studens.Add((await api.GetModels<UserModel>(WebApiTableNames.Users, $"ID = '{item.StudentID}'")).FirstOrDefault());

                foreach (var item in StorageFiles)
                    item.StudentFullName = Studens.Where(s => s.ID == item.StudentID).FirstOrDefault().FullName;
            }
            catch (Exception ex)
            {
                notification.ShowGlobalError("Во время загрузки произошла ошибка: " + ex.Message);
                throw;
            }
            finally
            {
                Loader = new();
            }
        }

        private void OnCloseModal()
        {
            manager.Regions[BaseMethods.RegionNames.ModalRegion].RemoveAll();
        }

        private async void OnDownloadFile(object parameter)
        {
            if (parameter is StorageFilesModel storageFile)
            {
                try
                {
                    FolderBrowserDialog folderFileDialog = new();
                    if (folderFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string pathFile = folderFileDialog.SelectedPath;

                        JsonFileModel file = (JsonFileModel)await api.GetFile($"Storage\\{CurrentStorage.ID}\\{storageFile.StudentID}\\{storageFile.FileName}");

                        File.WriteAllBytes(pathFile + $"\\{storageFile.FileName}", file.Data);
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
        #region navigation
        public void OnNavigatedTo(NavigationContext navigationContext) => CurrentStorage = navigationContext.Parameters.GetValue<StoragesHeadersModel>("storage");

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }
        #endregion
    }
}
