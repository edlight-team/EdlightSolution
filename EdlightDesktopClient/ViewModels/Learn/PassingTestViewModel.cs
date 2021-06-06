using ApplicationModels.Models;
using ApplicationServices.WebApiService;
using ApplicationWPFServices.MemoryService;
using ApplicationWPFServices.NotificationService;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Styles.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows;

namespace EdlightDesktopClient.ViewModels.Learn
{
    public class PassingTestViewModel : BindableBase, INavigationAware
    {
        #region services
        private INotificationService notification;
        private IMemoryService memory;
        private IWebApiService api;
        private IRegionManager manager;
        #endregion
        #region fields
        private LoaderModel testLoader;

        private TestHeadersModel testHeader;
        private ObservableCollection<QuestionsModel> questions;
        private TestResultsModel testResult;

        private TimeSpan timeTest;
        private Timer testTimer;
        private int minutes;
        private string timeHeader;
        private bool testTimeOver;

        private QuestionsModel currenQuestion;

        private Visibility nextButtonVisibility;
        private Visibility backButtonVisibility;
        #endregion
        #region props
        public LoaderModel TestLoader { get => testLoader; set => SetProperty(ref testLoader, value); }

        public TestHeadersModel TestHeader { get => testHeader; set => SetProperty(ref testHeader, value); }
        public ObservableCollection<QuestionsModel> Questions { get => questions; set => SetProperty(ref questions, value); }
        public TestResultsModel TestResult { get => testResult; set => SetProperty(ref testResult, value); }

        public string TimeHeader { get => timeHeader; set => SetProperty(ref timeHeader, value); }

        public QuestionsModel CurrenQuestion { get => currenQuestion; set => SetProperty(ref currenQuestion, value); }

        public Visibility NextButtonVisibility { get => nextButtonVisibility; set => SetProperty(ref nextButtonVisibility, value); }
        public Visibility BackButtonVisibility { get => backButtonVisibility; set => SetProperty(ref backButtonVisibility, value); }
        #endregion
        #region command
        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand CompleteTestCommand { get; private set; }

        public DelegateCommand NextQuestionCommand { get; private set; }
        public DelegateCommand BackQuestionCommand { get; private set; }
        #endregion
        #region constructor
        public PassingTestViewModel(INotificationService notification, IMemoryService memory, IWebApiService api, IRegionManager manager)
        {
            this.notification = notification;
            this.memory = memory;
            this.api = api;
            this.manager = manager;

            LoadedCommand = new(OnLoaded);
            CompleteTestCommand = new(OnCompleteTest);

            NextQuestionCommand = new(OnNextQuestion);
            BackQuestionCommand = new(OnBackQuestion);
        }
        #endregion
        #region methods

        #region loaded
        private async void OnLoaded()
        {
            try
            {
                TestLoader = new();

                TestsModel test = (await api.GetModels<TestsModel>(WebApiTableNames.Tests, $"ID = '{TestHeader.TestID}'")).FirstOrDefault();
                Questions = new(JsonConvert.DeserializeObject<List<QuestionsModel>>(test.Questions));
                UserModel currentUser = memory.GetItem<UserModel>(MemoryAlliases.CurrentUser);
                TestResult = (await api.GetModels<TestResultsModel>(WebApiTableNames.TestResults, $"UserID = '{currentUser.ID}'")).FirstOrDefault();

                for (int i = 0; i < Questions.Count; i++)
                    Questions[i].NumberQuestion = i + 1;

                CurrenQuestion = Questions[0];

                NextButtonVisibility = Questions.Count > 1 ? Visibility.Visible : Visibility.Hidden;
                BackButtonVisibility = Visibility.Hidden;

                DateTime time = Convert.ToDateTime(TestHeader.TestTime);
                timeTest = time.TimeOfDay;

                TimeHeader = "00:00";
                StartTimer();
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
        #region timer
        private void StartTimer()
        {
            testTimer = new Timer(60000);
            testTimer.Elapsed += (s, e) => Application.Current.Dispatcher.Invoke(() => OnTimedEvent(s, e));
            testTimer.AutoReset = true;
            testTimer.Enabled = true;
            testTimer.Start();
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            minutes += 1;
            if (minutes < 60)
                TimeHeader = new TimeSpan(0, minutes, 0).ToString(@"hh\:mm");
            else
                TimeHeader = new TimeSpan(minutes / 60, minutes % 60, 0).ToString(@"hh\:mm");

            if (timeTest.TotalMinutes == minutes)
            {
                testTimeOver = true;
                OnCompleteTest();
            }
        }
        #endregion
        #region command methods
        private async void OnCompleteTest()
        {
            try
            {
                if (!testTimeOver)
                {
                    bool? confirm = notification.ShowQuestion("Вы уверены что хотите завершить тест");
                    if (!confirm.HasValue || !confirm.Value) return;
                }

                TestResult.CorrectAnswers = CountNumberCorrectAnswers();
                TestResult.TestCompleted = true;

                await api.PutModel(TestResult, WebApiTableNames.TestResults);

                notification.ShowInformation($"Тест окончен. \r\nБаллы {TestResult.CorrectAnswers}/{TestHeader.CountPoints}");

                manager.Regions[BaseMethods.RegionNames.ModalRegion].RemoveAll();
            }
            catch (Exception ex)
            {
                notification.ShowError("Во время загрузки произошла ошибка: " + ex.Message);
                throw;
            }
        }

        private void OnNextQuestion()
        {
            int index = Questions.IndexOf(CurrenQuestion);
            CurrenQuestion = Questions[++index];
            ChangeButtonEnabled(index);
        }

        private void OnBackQuestion()
        {
            int index = Questions.IndexOf(CurrenQuestion);
            CurrenQuestion = Questions[--index];
            ChangeButtonEnabled(index);
        }

        private void ChangeButtonEnabled(int currentQuestionIndex)
        {
            if (currentQuestionIndex + 1 == Questions.Count)
                NextButtonVisibility = Visibility.Hidden;
            else if (NextButtonVisibility == Visibility.Hidden)
                NextButtonVisibility = Visibility.Visible;

            if (currentQuestionIndex == 0)
                BackButtonVisibility = Visibility.Hidden;
            else if (BackButtonVisibility == Visibility.Hidden)
                BackButtonVisibility = Visibility.Visible;

        }
        #endregion
        #region counting correct answers
        private int CountNumberCorrectAnswers()
        {
            int points = 0;
            foreach (var question in Questions)
                foreach (var answer in question.AnswerOptions)
                    if (answer.CorrectAnswer && answer.IsUserAnswer)
                    {
                        points += question.NumberPoints;
                        break;
                    }
            return points;
        }
        #endregion

        #endregion
        #region navigation
        public void OnNavigatedTo(NavigationContext navigationContext) => TestHeader = navigationContext.Parameters.GetValue<TestHeadersModel>("header");
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }
        #endregion
    }
}
