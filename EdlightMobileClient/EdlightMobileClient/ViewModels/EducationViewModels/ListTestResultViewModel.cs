using ApplicationModels.Models;
using ApplicationServices.WebApiService;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EdlightMobileClient.ViewModels.EducationViewModels
{
    public class ListTestResultViewModel : ViewModelBase
    {
        #region services
        private IWebApiService api;
        #endregion
        #region fields
        private ObservableCollection<TestResultsModel> testResults;
        private TestHeadersModel testHeader;
        #endregion
        #region props
        public ObservableCollection<TestResultsModel> TestResults { get => testResults ??= new(); set => SetProperty(ref testResults, value); }
        public TestHeadersModel TestHeader { get => testHeader ??= new(); set => SetProperty(ref testHeader, value); }
        #endregion
        #region constructor
        public ListTestResultViewModel(INavigationService navigationService, IWebApiService api) : base(navigationService)
        {
            this.api = api;
        }
        #endregion
        #region methods

        #endregion
        #region navigate
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            TestHeader = parameters.GetValue<TestHeadersModel>("header");
            List<TestResultsModel> results = await api.GetModels<TestResultsModel>(WebApiTableNames.TestResults, $"TestID = '{TestHeader.TestID}'");
            foreach (var item in results)
                TestResults.Add(item);
        }
        #endregion
    }
}
