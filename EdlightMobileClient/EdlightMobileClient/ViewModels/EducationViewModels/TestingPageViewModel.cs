using ApplicationModels.Models;
using ApplicationServices.WebApiService;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;

namespace EdlightMobileClient.ViewModels.EducationViewModels
{
    public class TestingPageViewModel : ViewModelBase,IDisposable
    {
        #region services
        private IWebApiService api;
        #endregion
        #region filds
        private int timeMinutes;
        private TestsModel testModel;
        private TestHeadersModel testHeader;
        private TestResultsModel result;
        private ObservableCollection<QuestionsModel> questions;
        private string timeHeader;

        private bool indicatorIsRunning;

        private TimeSpan timeTest;
        private Timer timer;
        #endregion
        #region props
        public ObservableCollection<QuestionsModel> Questions { get => questions; set => SetProperty(ref questions, value); }
        public TestHeadersModel TestHeader { get => testHeader; set => SetProperty(ref testHeader, value); }
        public TestResultsModel Result { get => result; set => SetProperty(ref result, value); }
        public string TimeHeader { get => timeHeader; set => SetProperty(ref timeHeader, value); }
        public bool IndicatorIsRunning { get => indicatorIsRunning; set => SetProperty(ref indicatorIsRunning, value); }
        #endregion
        #region commands
        public DelegateCommand NavigateCommand { get; private set; }
        #endregion
        #region constructor
        public TestingPageViewModel(INavigationService navigationService, IWebApiService api) : base(navigationService)
        {
            this.api = api;

            Questions = new();
            TimeHeader = new TimeSpan(0, 0, 0).ToString(@"hh\:mm");

            NavigateCommand = new DelegateCommand(EndTest);

            StartTimer();
        }
        #endregion
        #region methods
        private async void EndTest()
        {
            Result.CorrectAnswers = CountNumberCorrectAnswers();
            Result.TestCompleted = true;

            await api.PutModel(Result, WebApiTableNames.TestResults);
            
            NavigationParameters parametr = new()
            {
                { "header", TestHeader},
                { "result", Result}
            };
            await NavigationService.GoBackAsync(parametr);
        }

        private int CountNumberCorrectAnswers()
        {
            int counter = 0;
            foreach (var question in Questions)
                if (question.AnswerOptions[question.CorrectAnswerIndex].IsUserAnswer)
                    ++counter;
            return counter;
        }

        private void StartTimer()
        {
            timer = new Timer(60000);
            timer.Elapsed += (s, e) => Device.BeginInvokeOnMainThread (() => OnTimedEvent(s, e));
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            timeMinutes += 1;
            if (timeMinutes < 60)
            {
                TimeHeader = new TimeSpan(0, timeMinutes, 0).ToString(@"hh\:mm");
            }
            else
            {
                int hourse = timeMinutes / 60;
                TimeHeader = new TimeSpan(hourse, timeMinutes - hourse * 60, 0).ToString(@"hh\:mm");
            }
            if (timeTest.TotalMinutes == timeMinutes)
            {
                EndTest();
            }
        }
        #endregion
        #region navigation
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            TestHeader = parameters.GetValue<TestHeadersModel>("header");
            Result = parameters.GetValue<TestResultsModel>("result");
            DateTime time = Convert.ToDateTime(TestHeader.TestTime);
            timeTest = time.TimeOfDay;
            List<QuestionsModel> questions = parameters.GetValue<List<QuestionsModel>>("questions");
            foreach (var item in questions)
                Questions.Add(item);
        }

        public void Dispose()
        {
            timer.Dispose();
        }
        #endregion
    }
}
