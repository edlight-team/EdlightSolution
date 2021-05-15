using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace ApplicationModels.Models
{
    public class QuestionsModel:BindableBase
    {
        #region fields
        private int numberQuestion;
        private string question;
        private ObservableCollection<TestAnswer> answerOptions;
        private int correctAnswerIndex;
        #endregion
        #region props
        public int NumberQuestion { get => numberQuestion; set => SetProperty(ref numberQuestion, value); }
        public string Question { get => question; set => SetProperty(ref question, value); }
        public ObservableCollection<TestAnswer> AnswerOptions { get => answerOptions; set => SetProperty(ref answerOptions, value); }
        public int CorrectAnswerIndex { get => correctAnswerIndex; set => SetProperty(ref correctAnswerIndex, value); }
        #endregion
    }

    public class TestAnswer : BindableBase
    {
        #region fields
        private bool isUserAnswer;
        private string answer;
        #endregion
        #region props
        public bool IsUserAnswer { get => isUserAnswer; set => SetProperty(ref isUserAnswer, value); }
        public string Answer { get => answer; set => SetProperty(ref answer, value); }
        #endregion
    }
}
