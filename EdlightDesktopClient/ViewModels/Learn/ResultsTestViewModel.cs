using ApplicationModels.Models;
using ApplicationServices.WebApiService;
using ApplicationWPFServices.NotificationService;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Styles.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdlightDesktopClient.ViewModels.Learn
{
    public class ResultsTestViewModel : BindableBase, INavigationAware
    {
        #region services
        private IRegionManager manager;
        private IWebApiService api;
        private INotificationService notification;
        #endregion
        #region fields
        private LoaderModel resultLoader;
        private TestHeadersModel testHeader;

        private string testName;
        private string groupName;
        private int countPoints;
        private ObservableCollection<TestResultsModel> testResults;
        #endregion
        #region props
        public LoaderModel ResultLoader { get => resultLoader; set => SetProperty(ref resultLoader, value); }
        public string TestName { get => testName; set => SetProperty(ref testName, value); }
        public string GroupName { get => groupName; set => SetProperty(ref groupName, value); }
        public int CountPoints { get => countPoints; set => SetProperty(ref countPoints, value); }
        public ObservableCollection<TestResultsModel> TestResults { get => testResults; set => SetProperty(ref testResults, value); }
        #endregion
        #region commands
        public DelegateCommand CloseModalCommand { get; private set; }
        public DelegateCommand LoadedCommand { get; private set; }
        #endregion
        #region constructor
        public ResultsTestViewModel(IRegionManager manager, IWebApiService api, INotificationService notification)
        {
            this.manager = manager;
            this.api = api;
            this.notification = notification;

            CloseModalCommand = new(OnCloseModal);
            LoadedCommand = new(OnLoaded);
        }
        #endregion
        #region methods
        private async void OnLoaded()
        {
            try
            {
                ResultLoader = new("Выполняется загрузка");

                TestResults = new(await api.GetModels<TestResultsModel>(WebApiTableNames.TestResults, $"TestID = '{testHeader.TestID}'"));
            }
            catch (Exception ex)
            {
                notification.ShowError("Во время загрузки произошла ошибка: " + ex.Message);
                throw;
            }
            finally
            {
                ResultLoader = new();
            }
        }
        private void OnCloseModal()
        {
            manager.Regions[BaseMethods.RegionNames.ModalRegion].RemoveAll();
        }
        #endregion
        #region navigation
        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            testHeader = navigationContext.Parameters.GetValue<TestHeadersModel>("header");
            GroupsModel group = (await api.GetModels<GroupsModel>(WebApiTableNames.Groups, $"Id = '{testHeader.GroupID}'")).FirstOrDefault();
            GroupName = group.Group;
            TestName = testHeader.TestName;
            CountPoints = testHeader.CountPoints;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }
        #endregion
    }
}
