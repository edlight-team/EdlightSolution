using ApplicationModels.Models;
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
    public class AddTestViewModel : BindableBase, INavigationAware
    {
        #region services
        INotificationService notification;
        IRegionManager manager;
        #endregion
        #region filed
        private LoaderModel testLoader;
        private Guid testID;
        private List<GroupsModel> groups;
        private string[] testTypes;

        private string testName;
        private string testType;
        private string testGroupName;
        private int testTime;

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
        public string TestGroupName { get => testGroupName; set => SetProperty(ref testGroupName, value); }
        public int TestTime { get => testTime; set => SetProperty(ref testTime, value); }

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
        #endregion
        #region constructor
        public AddTestViewModel(INotificationService notification, IRegionManager manager)
        {
            this.notification = notification;
            this.manager = manager;

            TestTypes = new[] { "Контрольна работа", "Экзамен", "Зачёт", "Тест"};

            LoadedCommand = new(OnLoaded);

            CloseModalCommand = new(OnCloseModal);
            AddQuestionCommand = new(OnAddQuestion);
            UpdateQuestionCommand = new(OnUpdateQuestion);
            DeleteQuestionCommand = new(OnDeleteQuestion);
            SaveQuestionCommand = new(OnSaveQuestion);

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
                TestLoader = new("Выполняется загрузка");

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
            Questions[updatedQuestionIndex] = (QuestionsModel)newQuestion.Clone();
            QuestionEdited = false;
        }

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

        private void OnSaveTest()
        {

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
