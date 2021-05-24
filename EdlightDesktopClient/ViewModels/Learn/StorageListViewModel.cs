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
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace EdlightDesktopClient.ViewModels.Learn
{
    public class StorageListViewModel : BindableBase
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
        private LoaderModel storageLoader;
        private TestConfig config;
        private UserModel currentUser;

        private ObservableCollection<GroupsModel> groups;
        private ObservableCollection<StoragesHeadersModel> storageCards;
        private List<StorageFilesModel> storageFiles;
        private CollectionViewSource filteredStorageCards;
        private GroupsModel selectedGroup;
        private StoragesHeadersModel selectedCardHeader;
        private StorageFilesModel selectedStorageFile;
        private bool selectedStorageFileAdded;
        private bool cardSelected;

        private readonly GroupsModel allStorage;
        #endregion
        #region props
        public LoaderModel StorageLoader { get => storageLoader; set => SetProperty(ref storageLoader, value); }
        public TestConfig Config { get => config; set => SetProperty(ref config, value); }

        public ObservableCollection<GroupsModel> Groups { get => groups; set => SetProperty(ref groups, value); }
        public ObservableCollection<StoragesHeadersModel> StorageCards { get => storageCards ??= new(); set => SetProperty(ref storageCards, value); }
        public CollectionViewSource FilteredStorageCards { get => filteredStorageCards ??= new(); set => SetProperty(ref filteredStorageCards, value); }
        public StoragesHeadersModel SelectedCardHeader { get => selectedCardHeader ??= new(); set => SetProperty(ref selectedCardHeader, value); }
        public StorageFilesModel SelectedStorageFile { get => selectedStorageFile ??= new(); set => SetProperty(ref selectedStorageFile, value); }
        private bool SelectedStorageFileAdded { get => selectedStorageFileAdded; set => SetProperty(ref selectedStorageFileAdded, value); }
        public GroupsModel SelectedGroup
        {
            get => selectedGroup ??= new();
            set
            {
                SetProperty(ref selectedGroup, value);
                FilteredStorageCards.View?.Refresh();
            }
        }
        public bool CardSelected { get => cardSelected; set => SetProperty(ref cardSelected, value); }
        #endregion
        #region command
        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand<object> CardClickCommand { get; private set; }
        public DelegateCommand CreateStorageCommand { get; private set; }
        public DelegateCommand DeleteStorageCommand { get; private set; }
        public DelegateCommand ViewStorageFilesCommand { get; private set; }
        public DelegateCommand FileAddCommand { get; private set; }
        #endregion
        #region constructor
        public StorageListViewModel(IRegionManager manager, IMemoryService memory, IWebApiService api, INotificationService notification, IPermissionService permission, IEventAggregator aggregator)
        {
            this.manager = manager;
            this.memory = memory;
            this.api = api;
            this.notification = notification;
            this.permission = permission;
            this.aggregator = aggregator;

            allStorage = new() { Group = "Все группы" };

            LoadedCommand = new(OnLoaded);

            CardClickCommand = new(OnCardClick);
            CreateStorageCommand = new(OnCreateStorage);
            DeleteStorageCommand = new(OnDeleteStorage);
            ViewStorageFilesCommand = new(OnViewStorageFiles);
            FileAddCommand = new(OnFileAdd);

            aggregator.GetEvent<StorageCollectionUpdatedEvent>().Subscribe(OnLoaded);
        }
        #endregion
        #region methods
        #region loaded
        private async void OnLoaded()
        {
            try
            {
                StorageLoader = new();

                currentUser = memory.GetItem<UserModel>(MemoryAlliases.CurrentUser);
                await permission.ConfigureService(api, currentUser);
                RolesModel student_role = await permission.GetRoleByName("student");
                RolesModel teacher_role = await permission.GetRoleByName("teacher");

                Task loading_task = Task.Run(async () => await LoadingData());
                await Task.WhenAll(loading_task);

                FilteredStorageCards.Source = StorageCards;

                if (await permission.IsInRole(student_role))
                {
                    FilteredStorageCards.Filter += CardFilterStudent;
                    List<StudentsGroupsModel> studentsGroup = await api.GetModels<StudentsGroupsModel>(WebApiTableNames.StudentsGroups, $"IdStudent = '{currentUser.ID}'");
                    SelectedGroup = Groups.FirstOrDefault(g => g.Id == studentsGroup[0].IdGroup);
                }
                if (await permission.IsInRole(teacher_role))
                {
                    FilteredStorageCards.Filter += CardsFilter;
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
                StorageLoader = new();
            }
        }
        #endregion
        private void CardsFilter(object sender, FilterEventArgs e)
        {
            if (SelectedGroup != null)
            {
                StoragesHeadersModel card = e.Item as StoragesHeadersModel;
                if (!Equals(SelectedGroup, allStorage))
                {
                    if (card.GroupID.ToString().ToLower() != selectedGroup.Id.ToString().ToLower())
                    {
                        e.Accepted = false;
                    }
                }
                if (card.CreatorID != currentUser.ID)
                {
                    e.Accepted = false;
                }
            }
        }

        private void CardFilterStudent(object sender, FilterEventArgs e)
        {
            if (SelectedGroup != null)
            {
                StoragesHeadersModel card = e.Item as StoragesHeadersModel;
                if (card.GroupID.ToString().ToLower() != selectedGroup.Id.ToString().ToLower())
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
            Application.Current.Dispatcher.Invoke(() =>
            {
                Groups.Add(allStorage);
                foreach (var item in gr)
                    Groups.Add(item);
            });
            StorageCards = new ObservableCollection<StoragesHeadersModel>(await api.GetModels<StoragesHeadersModel>(WebApiTableNames.StoragesHeaders));
            storageFiles = await api.GetModels<StorageFilesModel>(WebApiTableNames.StorageFiles);
        }
        #region command methods
        private void OnCardClick(object parameter)
        {
            if (parameter is StoragesHeadersModel card)
            {
                if (!card.IsSelectedCard)
                {
                    card.IsSelectedCard = true;
                    CardSelected = true;
                    SelectedCardHeader = card;
                    SelectedStorageFile = storageFiles.Where(r => r.StorageID == card.ID && r.StudentID == currentUser.ID).FirstOrDefault();
                    if (string.IsNullOrEmpty(SelectedStorageFile.FileName))
                        SelectedStorageFileAdded = false;
                    else
                        SelectedStorageFileAdded = true;
                }
            }
        }
        #region добавление, удаление, изменение тестов
        private void OnCreateStorage() => manager.RequestNavigate(BaseMethods.RegionNames.ModalRegion, nameof(AddStorageView));

        private async void OnDeleteStorage()
        {
            try
            {
                bool isStorageEmpty = true;
                List<StorageFilesModel> model = await api.GetModels<StorageFilesModel>(WebApiTableNames.StorageFiles, $"StorageID = '{SelectedCardHeader.ID}'");

                foreach (var item in model)
                {
                    if (!string.IsNullOrEmpty(item.FileName))
                    {
                        isStorageEmpty = false;
                        break;
                    }
                }

                foreach (var item in model)
                    await api.DeleteModel(item.ID, WebApiTableNames.StorageFiles);
                if (!isStorageEmpty)
                    await api.DeleteFile($"Storage\\{SelectedCardHeader.ID}");

                await api.DeleteModel(SelectedCardHeader.ID, WebApiTableNames.StoragesHeaders);
            }
            catch (Exception ex)
            {
                notification.ShowGlobalError("Во время загрузки произошла ошибка: " + ex.Message);
                throw;
            }
            finally
            {
                StorageCards.Remove(SelectedCardHeader);
                notification.ShowGlobalInformation("Хранилище удалено");
                FilteredStorageCards.View?.Refresh();
            }
        }

        private void OnViewStorageFiles()
        {
            NavigationParameters parameter = new()
            {
                { "storage", SelectedCardHeader }
            };
            manager.RequestNavigate(BaseMethods.RegionNames.ModalRegion, nameof(StorageFileListView), parameter);
        }

        private async void OnFileAdd()
        {
            bool fileLoaded = false;
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                string filePath = string.Empty;
                string fileName = string.Empty;
                byte[] data;

                if (openFileDialog.ShowDialog() == true)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    fileName = openFileDialog.SafeFileName;

                    data = File.ReadAllBytes(filePath);

                    JsonFileModel fileModel = new()
                    {
                        FileName = fileName,
                        Data = data
                    };

                    StorageFilesModel storageFiles = (await api.GetModels<StorageFilesModel>(WebApiTableNames.StorageFiles, $"StorageID = '{SelectedCardHeader.ID}' and StudentID = '{currentUser.ID}'")).FirstOrDefault();
                    if (!string.IsNullOrEmpty(storageFiles.FileName))
                        await api.DeleteFile($"Storage\\{SelectedCardHeader.ID}\\{currentUser.ID}\\{storageFiles.FileName}");

                    await api.PushFile($"Storage\\{SelectedCardHeader.ID}\\{currentUser.ID}\\", fileModel);

                    
                    storageFiles.FileName = fileName;
                    await api.PutModel(storageFiles, WebApiTableNames.StorageFiles);

                    fileLoaded = true;
                }
            }
            catch (Exception ex)
            {
                notification.ShowGlobalError("Во время загрузки произошла ошибка: " + ex.Message);
                throw;
            }
            finally
            {
                if (fileLoaded)
                {
                    notification.ShowGlobalInformation("Файл загружен");
                    FilteredStorageCards.View?.Refresh();
                    CardSelected = false;
                }
            }
        }
        #endregion

        public static DateTime GetNetworkTime()
        {
            const string ntpServer = "time.windows.com";
            var ntpData = new byte[48];
            ntpData[0] = 0x1B;

            var addresses = Dns.GetHostEntry(ntpServer).AddressList;
            var ipEndPoint = new IPEndPoint(addresses[0], 123);

            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                socket.Connect(ipEndPoint);
                socket.Send(ntpData);
                socket.Receive(ntpData);
                socket.Close();
            }

            var intPart = (ulong)ntpData[40] << 24 | (ulong)ntpData[41] << 16 | (ulong)ntpData[42] << 8 | ntpData[43];
            var fractPart = (ulong)ntpData[44] << 24 | (ulong)ntpData[45] << 16 | (ulong)ntpData[46] << 8 | ntpData[47];

            var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
            var networkDateTime = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds((long)milliseconds);

            return networkDateTime.ToLocalTime();
        }
        #endregion
        #endregion
    }
}
