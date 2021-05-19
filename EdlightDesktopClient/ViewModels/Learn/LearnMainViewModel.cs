using ApplicationModels.Models;
using ApplicationServices.PermissionService;
using ApplicationServices.WebApiService;
using ApplicationWPFServices.MemoryService;
using ApplicationWPFServices.NotificationService;
using EdlightDesktopClient.AccessConfigurations;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Styles.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace EdlightDesktopClient.ViewModels.Learn
{
    public class LearnMainViewModel : BindableBase
    {
        #region services
        private IRegionManager manager;
        private IMemoryService memory;
        private IWebApiService api;
        private INotificationService notification;
        private IPermissionService permission;
        #endregion
        #region fields
        private LoaderModel testLoader;
        private TestConfig config;
        private UserModel currentUser;

        private ObservableCollection<GroupsModel> groups;
        private ObservableCollection<TestHeadersModel> testCards;
        private ObservableCollection<TestResultsModel> testResults;
        private CollectionViewSource filteredTestCards;
        private GroupsModel selectedGroup;
        private TestHeadersModel selectedCardHeader;
        private bool cardSelected;

        private readonly GroupsModel allTests;
        #endregion
        #region props
        public LoaderModel TestLoader { get => testLoader; set => SetProperty(ref testLoader, value); }
        public TestConfig Config { get => config; set => SetProperty(ref config, value); }

        public ObservableCollection<GroupsModel> Groups { get => groups; set => SetProperty(ref groups, value); }
        public ObservableCollection<TestHeadersModel> TestCards { get => testCards ??= new(); set => SetProperty(ref testCards, value); }
        public ObservableCollection<TestResultsModel> TestResults { get => testResults ??= new(); set => SetProperty(ref testResults, value); }
        public CollectionViewSource FilteredTestCards { get => filteredTestCards ??= new(); set => SetProperty(ref filteredTestCards, value); }
        public TestHeadersModel SelectedCardHeader { get => selectedCardHeader ??= new(); set => SetProperty(ref selectedCardHeader, value); }
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
        public LearnMainViewModel(IRegionManager manager, IMemoryService memory, IWebApiService api, INotificationService notification, IPermissionService permission)
        {
            this.manager = manager;
            this.memory = memory;
            this.api = api;
            this.notification = notification;
            this.permission = permission;

            allTests = new() { Group = "Все группы" };

            LoadedCommand = new(OnLoaded);

            CardClickCommand = new(OnCardClick);
            AddTestCommand = new(OnAddTest);
            UpdateTestCommand = new(OnUpdateTest);
            DeleteTestCommand = new(OnDeleteTest);
            StartTestCommand = new(OnStartTest);
            ViewTestResultsCommand = new(OnViewTestResults);
        }
        #endregion
        #region methods
        #region loaded
        private async void OnLoaded()
        {
            try
            {
                TestLoader = new("Выполняется загрузка");

                currentUser = memory.GetItem<UserModel>(MemoryAlliases.CurrentUser);
                await permission.ConfigureService(api, currentUser);
                RolesModel student_role = await permission.GetRoleByName("student");

                Task loading_task = Task.Run(async () => await LoadingData());
                await Task.WhenAll(loading_task);

                FilteredTestCards.Source = TestCards;

                if (await permission.IsInRole(student_role))
                {
                    FilteredTestCards.Filter += CardFilterStudent;
                    List<StudentsGroupsModel> studentsGroup = await api.GetModels<StudentsGroupsModel>(WebApiTableNames.StudentsGroups, $"IdStudent = '{currentUser.ID}'");
                    selectedGroup = Groups.FirstOrDefault(g => g.Id == studentsGroup[0].IdGroup);
                }
                else
                {
                    FilteredTestCards.Filter += CardsFilter;
                    SelectedGroup = allTests;
                }
            }
            catch (Exception ex)
            {
                notification.ShowError("Во время загрузки произошла ошибка: " + ex.Message);
                throw;
            }
            finally
            {
                TestLoader = new();
            }
        }
        #endregion
        private void CardsFilter(object sender, FilterEventArgs e)
        {
            if (SelectedGroup != null)
            {
                TestHeadersModel card = e.Item as TestHeadersModel;
                if (card.TeacherID != currentUser.ID)
                {
                    if (card.GroupID.ToString().ToLower() != selectedGroup.Id.ToString().ToLower() ||
                        !Equals(SelectedGroup, allTests))
                    {
                        e.Accepted = false;
                    }
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
            TestResults = new ObservableCollection<TestResultsModel>(await api.GetModels<TestResultsModel>(WebApiTableNames.TestResults));
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
                }
            }
        }

        private void OnStartTest(object parameter)
        {
            if (parameter is TestHeadersModel card)
            {
                card.IsSelectedCard = true;
                SelectedCardHeader = card;
            }
        }

        private void OnViewTestResults()
        {

        }

        private void OnAddTest()
        {

        }

        private void OnUpdateTest()
        {

        }

        private void OnDeleteTest()
        {

        }
        #endregion
        #endregion
    }
}
