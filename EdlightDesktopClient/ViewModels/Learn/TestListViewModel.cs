using ApplicationEventsWPF.Events;
using ApplicationModels.Models;
using ApplicationServices.PermissionService;
using ApplicationServices.WebApiService;
using ApplicationWPFServices.MemoryService;
using ApplicationWPFServices.NotificationService;
using EdlightDesktopClient.AccessConfigurations;
using EdlightDesktopClient.Views.Learn;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Styles.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace EdlightDesktopClient.ViewModels.Learn
{
    [RegionMemberLifetime(KeepAlive = true)]
    public class TestListViewModel : BindableBase
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
        private LoaderModel testLoader;
        private TestConfig config;
        private UserModel currentUser;

        private ObservableCollection<GroupsModel> groups;
        private ObservableCollection<TestHeadersModel> testCards;
        private List<TestResultsModel> testResults;
        private CollectionViewSource filteredTestCards;
        private GroupsModel selectedGroup;
        private TestHeadersModel selectedCardHeader;
        private TestResultsModel selectedTestResult;
        private bool cardSelected;

        private readonly GroupsModel allTests;
        #endregion
        #region props
        public LoaderModel TestLoader { get => testLoader; set => SetProperty(ref testLoader, value); }
        public TestConfig Config { get => config; set => SetProperty(ref config, value); }

        public ObservableCollection<GroupsModel> Groups { get => groups; set => SetProperty(ref groups, value); }
        public ObservableCollection<TestHeadersModel> TestCards { get => testCards ??= new(); set => SetProperty(ref testCards, value); }
        public CollectionViewSource FilteredTestCards { get => filteredTestCards ??= new(); set => SetProperty(ref filteredTestCards, value); }
        public TestHeadersModel SelectedCardHeader { get => selectedCardHeader ??= new(); set => SetProperty(ref selectedCardHeader, value); }
        public TestResultsModel SelectedTestResult { get => selectedTestResult ??= new(); set => SetProperty(ref selectedTestResult, value); }
        public GroupsModel SelectedGroup
        {
            get => selectedGroup ??= new();
            set
            {
                SetProperty(ref selectedGroup, value);
                FilteredTestCards.View?.Refresh();
            }
        }
        public bool CardSelected { get => cardSelected; set => SetProperty(ref cardSelected, value); }
        #endregion
        #region command
        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand<object> CardClickCommand { get; private set; }
        public DelegateCommand AddTestCommand { get; private set; }
        public DelegateCommand UpdateTestCommand { get; private set; }
        public DelegateCommand DeleteTestCommand { get; private set; }
        public DelegateCommand<object> StartTestCommand { get; private set; }
        public DelegateCommand ViewTestResultsCommand { get; private set; }
        #endregion
        #region constructor
        public TestListViewModel(IRegionManager manager, IMemoryService memory, IWebApiService api, INotificationService notification, IPermissionService permission, IEventAggregator aggregator)
        {
            TestLoader = new();
            this.manager = manager;
            this.memory = memory;
            this.api = api;
            this.notification = notification;
            this.permission = permission;
            this.aggregator = aggregator;

            allTests = new() { Group = "Все группы" };

            LoadedCommand = new(OnLoaded);

            CardClickCommand = new(OnCardClick);
            AddTestCommand = new(OnAddTest);
            UpdateTestCommand = new(OnUpdateTest);
            DeleteTestCommand = new(OnDeleteTest);
            StartTestCommand = new(OnStartTest);
            ViewTestResultsCommand = new(OnViewTestResults);

            aggregator.GetEvent<TestCollectionUpdatedEvent>().Subscribe(OnLoaded);
        }
        #endregion
        #region methods
        #region loaded
        private async void OnLoaded()
        {
            try
            {
                TestLoader.SetDefaultLoadingInfo();

                currentUser = memory.GetItem<UserModel>(MemoryAlliases.CurrentUser);
                //await permission.ConfigureService(api, currentUser);
                RolesModel student_role = await permission.GetRoleByName("student");
                RolesModel teacher_role = await permission.GetRoleByName("teacher");

                Task loading_task = Task.Run(async () => await LoadingData());
                await Task.WhenAll(loading_task);

                FilteredTestCards.Source = TestCards;

                if (await permission.IsInRole(student_role))
                {
                    FilteredTestCards.Filter += CardFilterStudent;
                    List<StudentsGroupsModel> studentsGroup = await api.GetModels<StudentsGroupsModel>(WebApiTableNames.StudentsGroups, $"IdStudent = '{currentUser.ID}'");
                    SelectedGroup = Groups.FirstOrDefault(g => g.Id == studentsGroup[0].IdGroup);
                }
                if(await permission.IsInRole(teacher_role))
                {
                    FilteredTestCards.Filter += CardsFilter;
                    SelectedGroup = allTests;
                }
            }
            catch (Exception ex)
            {
                //notification.ShowError("Во время загрузки произошла ошибка: " + ex.Message);
                throw;
            }
            finally
            {
                await TestLoader.Clear();
            }
        }
        #endregion
        private void CardsFilter(object sender, FilterEventArgs e)
        {
            if (SelectedGroup != null)
            {
                TestHeadersModel card = e.Item as TestHeadersModel;
                if (!Equals(SelectedGroup, allTests))
                {
                    if (card.GroupID.ToString().ToLower() != selectedGroup.Id.ToString().ToLower())
                    {
                        e.Accepted = false;
                    }
                }
                if (card.TeacherID != currentUser.ID)
                {
                    e.Accepted = false;
                }
            }
        }

        private void CardFilterStudent(object sender, FilterEventArgs e)
        {
            if (SelectedGroup != null)
            {
                TestHeadersModel card = e.Item as TestHeadersModel;
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
                Groups.Add(allTests);
                foreach (var item in gr)
                    Groups.Add(item);
            });
            TestCards = new ObservableCollection<TestHeadersModel>(await api.GetModels<TestHeadersModel>(WebApiTableNames.TestHeaders));
            testResults = await api.GetModels<TestResultsModel>(WebApiTableNames.TestResults);
        }
        #region command methods
        private void OnCardClick(object parameter)
        {
            if (parameter is TestHeadersModel card)
            {
                if (!card.IsSelectedCard)
                {
                    card.IsSelectedCard = true;
                    CardSelected = true;
                    SelectedCardHeader = card;
                    SelectedTestResult = testResults.Where(r => r.TestID == card.TestID && r.UserID == currentUser.ID).FirstOrDefault();
                }
            }
        }

        private void OnStartTest(object parameter)
        {
            if (parameter is TestHeadersModel card)
            {
                try
                {
                    DateTime currentDateTime = GetNetworkTime();
                    if (DateTime.ParseExact(card.TestStartDate, "G", System.Globalization.CultureInfo.InvariantCulture) > currentDateTime)
                    {
                        notification.ShowInformation("Дождитесь начала теста");
                        return;
                    }
                    if (DateTime.ParseExact(card.TestEndDate, "G", System.Globalization.CultureInfo.InvariantCulture) < currentDateTime)
                    {
                        notification.ShowInformation("Время для прохождения теста закончилось");
                        return;
                    }
                    card.IsSelectedCard = true;
                    SelectedCardHeader = card;
                    SelectedTestResult = testResults.Where(r => r.TestID == card.TestID && r.UserID == currentUser.ID).FirstOrDefault();
                    if (SelectedTestResult.TestCompleted)
                        notification.ShowInformation("Тест уже пройден");
                    else
                    {
                        NavigationParameters navigationParameter = new()
                        {
                            { "header", SelectedCardHeader }
                        };
                        manager.RequestNavigate(BaseMethods.RegionNames.ModalRegion, nameof(PassingTestView), navigationParameter);
                    }
                }
                catch (Exception ex)
                {
                    notification.ShowError(ex.Message);
                }
                finally
                {
                    CardSelected = false;
                    SelectedCardHeader = new(); ;
                }
            }
        }

        private void OnViewTestResults()
        {
            NavigationParameters parameter = new()
            {
                { "header", SelectedCardHeader }
            };
            manager.RequestNavigate(BaseMethods.RegionNames.ModalRegion, nameof(ResultsTestView), parameter);
        }
        #region добавление, удаление, изменение тестов
        private void OnAddTest()
        {
            NavigationParameters parameter = new()
            {
                { "iscreate", true }
            };
            manager.RequestNavigate(BaseMethods.RegionNames.ModalRegion, nameof(AddTestView),parameter);
        }

        private void OnUpdateTest()
        {
            NavigationParameters parameter = new()
            {
                { "iscreate", false },
                { "testid", SelectedCardHeader.TestID }
            };
            manager.RequestNavigate(BaseMethods.RegionNames.ModalRegion, nameof(AddTestView), parameter);
        }

        private async void OnDeleteTest()
        {
            try
            {
                TestLoader.SetDefaultLoadingInfo();
                List<TestResultsModel> testResults = await api.GetModels<TestResultsModel>(WebApiTableNames.TestResults, $"TestID = '{SelectedCardHeader.TestID}'");
                foreach (var item in testResults)
                    await api.DeleteModel(item.ID, WebApiTableNames.TestResults);
                await api.DeleteModel(SelectedCardHeader.ID, WebApiTableNames.TestHeaders);
                await api.DeleteModel(SelectedCardHeader.TestID, WebApiTableNames.Tests);
            }
            catch (Exception ex)
            {
                notification.ShowError("Во время загрузки произошла ошибка: " + ex.Message);
                throw;
            }
            finally
            {
                await TestLoader.Clear();
                TestCards.Remove(SelectedCardHeader);
                FilteredTestCards.View?.Refresh();
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
