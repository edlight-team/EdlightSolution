using Prism.Mvvm;

namespace ApplicationModels.Models
{
    public class QuestionsModel : BindableBase
    {
        #region fields
        private int numberQuestion;
        private string question;
        private string[] answerOptions;
        private int correctAnswerIndex;
        #endregion
        #region props
        public int NumberQuestion { get => numberQuestion; set => SetProperty(ref numberQuestion, value); }
        public string Question { get => question; set => SetProperty(ref question, value); }
        public string[] AnswerOptions { get => answerOptions; set => SetProperty(ref answerOptions, value); }
        public int CorrectAnswerIndex { get => correctAnswerIndex; set => SetProperty(ref correctAnswerIndex, value); }
        #endregion
    }
}
