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
    public class TestingPageViewModel : ViewModelBase
    {
        #region filds
        private TestsModel testModel;
        private TestHeadersModel testHeader;
        private TestResultsModel result;
        private ObservableCollection<QuestionsModel> questions;
        #endregion
        #region props
        public ObservableCollection<QuestionsModel> Questions { get => questions; set => SetProperty(ref questions, value); }
        public TestHeadersModel TestHeader { get => testHeader; set => SetProperty(ref testHeader, value); }
        public TestResultsModel Result { get => result; set => SetProperty(ref result, value); }
        #endregion
        #region commands
        public DelegateCommand NavigateCommand { get; private set; }
        #endregion
        #region constructor
        public TestingPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            QuestionsModel questionOne = new()
            {
                NumberQuestion = 1,
                Question = "Вопрос",
                AnswerOptions = new string[]
                {
                    "ответ 1",
                    "ответ 2",
                    "ответ 3",
                    "ответ 4"
                },
                CorrectAnswerIndex = 1
            };
            Questions = new();
            Questions.Add(questionOne);
            Questions.Add(questionOne);
            Questions.Add(questionOne);

            NavigateCommand = new DelegateCommand(EndTest);
        }
        #endregion
        #region methods
        private async void EndTest()
        {
            Result.TestCompleted = true;
            NavigationParameters parametr = new()
            {
                { "header", TestHeader},
                { "result", Result}
            };
            await NavigationService.GoBackAsync(parametr);
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
