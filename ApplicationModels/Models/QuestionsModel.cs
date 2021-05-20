using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;

namespace ApplicationModels.Models
{
    public class QuestionsModel : BindableBase, ICloneable
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

        public object Clone()
        {
            System.Collections.Generic.List<TestAnswer> answers = new();
            foreach (var item in this.AnswerOptions)
                answers.Add((TestAnswer)item.Clone());

            return new QuestionsModel()
            {
                NumberQuestion = this.NumberQuestion,
                Question = string.Copy(this.Question),
                AnswerOptions = new(answers),
                CorrectAnswerIndex = this.CorrectAnswerIndex
            };
        }
        #endregion
    }

    public class TestAnswer : BindableBase, ICloneable
    {
        #region fields
        private bool isUserAnswer;
        private bool correctAnswer;
        private string answer;
        #endregion
        #region props
        public bool IsUserAnswer { get => isUserAnswer; set => SetProperty(ref isUserAnswer, value); }

        public bool CorrectAnswer { get => correctAnswer; set => SetProperty(ref correctAnswer, value); }

        public string Answer { get => answer; set => SetProperty(ref answer, value); }

        public object Clone()
        {
            return new TestAnswer() { CorrectAnswer = this.CorrectAnswer, Answer = string.Copy(this.Answer) };
        }
        #endregion
    }
}
