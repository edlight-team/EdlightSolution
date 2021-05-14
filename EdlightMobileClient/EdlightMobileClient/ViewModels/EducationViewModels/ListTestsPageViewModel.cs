using ApplicationModels.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EdlightMobileClient.ViewModels.EducationViewModels
{
    public class ListTestsPageViewModel : ViewModelBase
    {
        #region field
        private ObservableCollection<TestHeadersModel> testHeaders;
        private UserModel userModel;
        #endregion
        #region props
        public ObservableCollection<TestHeadersModel> TestHeaders { get => testHeaders; set => SetProperty(ref testHeaders, value); }
        #endregion
        #region commands
        public DelegateCommand<object> NavigateCommand { get; private set; }
        #endregion
        #region constructor
        public ListTestsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            DateTime time = new DateTime(2021, 3, 1, 1, 20, 0);
            TestHeadersModel testHeaderModel = new()
            {
                TestName = "Test1",
                TestType = "Контрольная работа",
                CountQuestions = 3
            };
            TestHeaders = new();
            TestHeaders.Add(testHeaderModel);
            TestHeaders.Add(testHeaderModel);
            TestHeaders.Add(testHeaderModel);
            NavigateCommand = new DelegateCommand<object>(NavidateToStarEndTestPage);
        }
        #endregion
        #region methods
        public async void NavidateToStarEndTestPage(object parametr)
        {
            if (parametr is TestHeadersModel testHeader)
            {
                NavigationParameters navigationParams = new()
                {
                    { "header", testHeader },
                    { "result", new TestResultsModel() { CorrectAnswers=0,TestCompleted=false} }
                };
                await NavigationService.NavigateAsync("StartEndTestPage", navigationParams);
            }
        }
        #endregion
    }
}
