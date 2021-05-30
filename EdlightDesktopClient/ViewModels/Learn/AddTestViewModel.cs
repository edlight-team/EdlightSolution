using ApplicationEventsWPF.Events.LearnEvents;
using ApplicationModels.Models;
using ApplicationServices.WebApiService;
using ApplicationWPFServices.MemoryService;
using ApplicationWPFServices.NotificationService;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Styles.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EdlightDesktopClient.ViewModels.Learn
{
    public class AddTestViewModel : BindableBase, INavigationAware
    {
        #region services
        private INotificationService notification;
        private IRegionManager manager;
        private IWebApiService api;
        private IMemoryService memory;
        private IEventAggregator aggregator;
        #endregion
        #region filed
        private LoaderModel testLoader;
        private UserModel currentUser;
        private Guid testID;
        private Guid testHeaderID;
        private List<GroupsModel> groups;
        private string[] testTypes;

        private string testName;
        private string testType;
        private GroupsModel testGroupName;
        private int testTime;
        private DateTime startDateTime;
        private DateTime endDateTime;

        private bool isCreateTest;
        private ObservableCollection<QuestionsModel> questions;
        private QuestionsModel newQuestion;
        private int updatedQuestionIndex;
        private string newAnswer;
        private bool questionEdited;
        #endregion
        #region props
        public List<GroupsModel> Groups { get => groups ??= new(); set => SetProperty(ref groups, value); }
        public string[] TestTypes { get => testTypes; set => SetProperty(ref testTypes, value); }

        public string TestName { get => testName; set => SetProperty(ref testName, value); }
        public string TestType { get => testType; set => SetProperty(ref testType, value); }
        public GroupsModel TestGroupName { get => testGroupName; set => SetProperty(ref testGroupName, value); }
        public int TestTime
        {
            get => testTime;
            set
            {
                if (value >= 0)
                    SetProperty(ref testTime, value);
                else
                    RaisePropertyChanged(nameof(TestTime));
            }
        }
        public DateTime StartDateTime { get => startDateTime; set => SetProperty(ref startDateTime, value); }
        public DateTime EndDateTime { get => endDateTime; set => SetProperty(ref endDateTime, value); }

        public LoaderModel TestLoader { get => testLoader; set => SetProperty(ref testLoader, value); }
        public ObservableCollection<QuestionsModel> Questions { get => questions ??= new(); set => SetProperty(ref questions, value); }
        public QuestionsModel NewQuestion { get => newQuestion; set => SetProperty(ref newQuestion, value); }
        public string NewAnswer { get => newAnswer; set => SetProperty(ref newAnswer, value); }
        public bool QuestionEdited { get => questionEdited; set => SetProperty(ref questionEdited, value); }
        #endregion
        #region commands
        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand CloseModalCommand { get; private set; }
        public DelegateCommand AddQuestionCommand { get; private set; }
        public DelegateCommand<object> UpdateQuestionCommand { get; private set; }
        public DelegateCommand<object> DeleteQuestionCommand { get; private set; }
        public DelegateCommand SaveQuestionCommand { get; private set; }
        public DelegateCommand AddAnswerCommand { get; private set; }
        public DelegateCommand<object> DeleteAnswerCommand { get; private set; }
        public DelegateCommand SaveTestCommand { get; private set; }
        public DelegateCommand CancelEditQuestionCommand { get; private set; }
        #endregion
        #region constructor
        public AddTestViewModel(INotificationService notification, IRegionManager manager, IWebApiService api, IMemoryService memory, IEventAggregator aggregator)
        {
            this.notification = notification;
            this.manager = manager;
            this.api = api;
            this.memory = memory;
            this.aggregator = aggregator;

            TestTypes = new[] { "Контрольна работа", "Экзамен", "Зачёт", "Тест"};

            currentUser = currentUser = memory.GetItem<UserModel>(MemoryAlliases.CurrentUser);

            LoadedCommand = new(OnLoaded);

            CloseModalCommand = new(OnCloseModal);
            AddQuestionCommand = new(OnAddQuestion);
            UpdateQuestionCommand = new(OnUpdateQuestion);
            DeleteQuestionCommand = new(OnDeleteQuestion);
            SaveQuestionCommand = new(OnSaveQuestion);
            CancelEditQuestionCommand = new(OnCancelEditQuestion);

            AddAnswerCommand = new(OnAddAnswer);
            DeleteAnswerCommand = new(OnDeleteAnswer);

            SaveTestCommand = new(OnSaveTest);
        }
        #endregion
        #region methods
        private async void OnLoaded()
        {
            try
            {
                TestLoader = new();

                Groups = await api.GetModels<GroupsModel>(WebApiTableNames.Groups);
                if (!isCreateTest)
                {
                    TestsModel test = (await api.GetModels<TestsModel>(WebApiTableNames.Tests, $"ID = '{testID}'")).FirstOrDefault();
                    TestHeadersModel testHeader = (await api.GetModels<TestHeadersModel>(WebApiTableNames.TestHeaders, $"TestID = '{testID}'")).FirstOrDefault();
                    testHeaderID = testHeader.ID;
                    TestName = testHeader.TestName;
                    TestType = testHeader.TestType;
                    TestGroupName = Groups.Where(g => g.Id == testHeader.GroupID).FirstOrDefault();
                    TestTime = (int)Convert.ToDateTime(testHeader.TestTime).TimeOfDay.TotalMinutes;
                    StartDateTime = Convert.ToDateTime(testHeader.TestStartDate);
                    EndDateTime = Convert.ToDateTime(testHeader.TestEndDate);
                    Questions = new(JsonConvert.DeserializeObject<List<QuestionsModel>>(test.Questions));
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

        private void OnCloseModal()=> manager.Regions[BaseMethods.RegionNames.ModalRegion].RemoveAll();

        #region Questions command method
        private void OnAddQuestion()
        {
            Questions.Add(new QuestionsModel() { Question = "Вопрос" });
        }
        private void OnUpdateQuestion(object parameter)
        {
            QuestionEdited = true;
            if (parameter is QuestionsModel question)
            {
                updatedQuestionIndex = Questions.IndexOf(question);
                NewQuestion = new();
                NewQuestion.Question = string.Copy(question.Question);
                NewQuestion.AnswerOptions = new();
                NewQuestion.NumberPoints = question.NumberPoints;
                if (question.AnswerOptions != null)
                    foreach (var item in question.AnswerOptions)
                        NewQuestion.AnswerOptions.Add((TestAnswer)item.Clone());
            }
        }
        private void OnDeleteQuestion(object parameter)
        {
            if (parameter is QuestionsModel question)
                Questions.Remove(question);
        }

        private void OnSaveQuestion()
        {
            bool correctAnswerSelect = false;
            foreach (var item in newQuestion.AnswerOptions)
            {
                if (item.CorrectAnswer)
                {
                    correctAnswerSelect = true;
                    break;
                }
            }
            if (!correctAnswerSelect)
            {
                notification.ShowInformation("Выберите правильный ответ");
                return;
            }
            if (newQuestion.NumberPoints <= 0)
            {
                notification.ShowInformation("Введите количество баллов за ответ");
                return;
            }
            Questions[updatedQuestionIndex] = (QuestionsModel)newQuestion.Clone();
            QuestionEdited = false;
        }

        private void OnCancelEditQuestion() => QuestionEdited = false;

        private void OnAddAnswer()
        {
            if (!string.IsNullOrEmpty(NewAnswer))
            {
                NewQuestion.AnswerOptions.Add(new TestAnswer() { Answer = string.Copy(NewAnswer) });
                NewAnswer = string.Empty;
            }
            else
                notification.ShowInformation("Введите ответ");
        }
        private void OnDeleteAnswer(object parameter)
        {
            if (parameter is TestAnswer answer)
                NewQuestion.AnswerOptions.Remove(answer);
        }

        private async void OnSaveTest()
        {
            if (string.IsNullOrEmpty(TestName))
            {
                notification.ShowInformation("Введите имя теста");
                return;
            }
            if (string.IsNullOrEmpty(TestType))
            {
                notification.ShowInformation("Выберите тип теста");
                return;
            }
            if (TestGroupName == null)
            {
                notification.ShowInformation("Выберите группу");
                return;
            }
            if (TestTime == 0)
            {
                notification.ShowInformation("Выберите время прохождения");
                return;
            }
            if (Questions.Count == 0)
            {
                notification.ShowInformation("Добавьте вопросы");
                return;
            }
            if (EndDateTime < StartDateTime)
            {
                notification.ShowInformation("Конец теста не может быть раньше чем его начало");
                return;
            }
            try
            {
                TestLoader = new();

                TestHeadersModel testHeader = new();
                TestsModel test = new();

                int countPoints = 0;
                foreach (var item in Questions)
                    countPoints += item.NumberPoints;

                testHeader.TestName = TestName;
                testHeader.TestType = TestType;
                int hours = TestTime / 60;
                int minutes = TestTime - hours * 60;
                testHeader.TestTime = Convert.ToDateTime(new DateTime(2020, 3, 1) + new TimeSpan(hours, minutes, 0)).ToShortTimeString();
                testHeader.TeacherID = currentUser.ID;
                testHeader.GroupID = TestGroupName.Id;
                testHeader.CountQuestions = Questions.Count;
                testHeader.TestStartDate = StartDateTime.ToString("G");
                testHeader.TestEndDate = EndDateTime.ToString("G");
                testHeader.CountPoints = countPoints;

                test.Questions = JsonConvert.SerializeObject(Questions.ToList());
                if (isCreateTest)
                {
                    test = await api.PostModel(test, WebApiTableNames.Tests);

                    testHeader.TestID = test.ID;
                    testHeader = await api.PostModel(testHeader, WebApiTableNames.TestHeaders);

                    List<StudentsGroupsModel> studentsGroups = await api.GetModels<StudentsGroupsModel>(WebApiTableNames.StudentsGroups, $"IdGroup = '{testHeader.GroupID}'");
                    List<UserModel> students = new();
                    foreach (var item in studentsGroups)
                        students.Add((await api.GetModels<UserModel>(WebApiTableNames.Users, $"ID = '{item.IdStudent}'")).FirstOrDefault());
                    foreach (var item in students)
                    {
                        await api.PostModel(new TestResultsModel()
                        {
                            UserID = item.ID,
                            StudentName = item.Name,
                            StudentSurname = item.Surname,
                            TestID = test.ID,
                            TestCompleted = false,
                            CorrectAnswers = 0
                        }, WebApiTableNames.TestResults);
                    }
                }
                else
                {
                    test.ID = testID;
                    await api.PutModel(test, WebApiTableNames.Tests);

                    testHeader.ID = testHeaderID;
                    testHeader.TestID = testID;
                    testHeader = await api.PutModel(testHeader, WebApiTableNames.TestHeaders);
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
                manager.Regions[BaseMethods.RegionNames.ModalRegion].RemoveAll();
                aggregator.GetEvent<TestCollectionUpdatedEvent>().Publish();
            }
        }
        #endregion

        #endregion
        #region navigation
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            isCreateTest = navigationContext.Parameters.GetValue<bool>("iscreate");
            if (!isCreateTest)
                testID= navigationContext.Parameters.GetValue<Guid>("testid");
        }
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }
        #endregion
    }
}
