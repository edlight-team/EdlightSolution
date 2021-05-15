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
        public ObservableCollection<TestResultsModel> TestResults { get => testResults; set => SetProperty(ref testResults, value); }
        public TestHeadersModel TestHeader { get => testHeader; set => SetProperty(ref testHeader, value); }
        #endregion
        #region constructor
        public ListTestResultViewModel(INavigationService navigationService, IWebApiService api) : base(navigationService)
        {
            this.api = api;

            TestHeader.TestName = "Имя теста";
            TestResultsModel test = new()
            {
                StudentName = "Человек",
                StudentSurname = "Человеков",
                CorrectAnswers=50,
                TestCompleted = true
            };
            TestResultsModel testo = new()
            {
                StudentName = "НеЧеловек",
                StudentSurname = "НеЧеловеков",
                CorrectAnswers = 1,
                TestCompleted = false
            };
            TestResults.Add(test);
            TestResults.Add(testo);
            TestResults.Add(test);
            TestResults.Add(testo);
            TestResults.Add(test);
            TestResults.Add(testo);
        }
        #endregion
        #region methods

        #endregion
        #region navigate
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            //TestHeader = parameters.GetValue<TestHeadersModel>("header");
            //List<TestResultsModel> results = parameters.GetValue<List<TestResultsModel>>("result");
            //foreach (var item in results)
            //    TestResults.Add(item);
        }
        #endregion
    }
}
