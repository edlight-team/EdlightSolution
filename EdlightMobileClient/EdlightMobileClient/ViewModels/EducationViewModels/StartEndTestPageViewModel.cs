using ApplicationModels.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EdlightMobileClient.ViewModels.EducationViewModels
{
    public class StartEndTestPageViewModel : ViewModelBase
    {
        #region fields
        private TestHeadersModel testHeader;
        private TestResultsModel result;
        #endregion
        #region props
        public TestHeadersModel TestHeader { get => testHeader; set => SetProperty(ref testHeader, value); }
        public TestResultsModel Result { get => result; set => SetProperty(ref result, value); }
        #endregion
        #region command
        public DelegateCommand<object> NavigateCommand { get; private set; }
        #endregion
        #region consrtuctor
        public StartEndTestPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            NavigateCommand = new DelegateCommand<object>(NaviagteToTestingPage);
        }
        #endregion
        #region methods
        public async void NaviagteToTestingPage(object parametr)
        {
            NavigationParameters navigationParams = new()
            {
                { "header", testHeader },
                { "result", result }
            };
            await NavigationService.NavigateAsync("TestingPage", navigationParams);
        }
        #endregion
        #region navigation
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            TestHeader = parameters.GetValue<TestHeadersModel>("header");
            Result = parameters.GetValue<TestResultsModel>("result");
        }
        #endregion
    }
}
