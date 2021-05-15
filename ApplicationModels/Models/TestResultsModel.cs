using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationModels.Models
{
    public class TestResultsModel:BindableBase
    {
        #region fields
        public Guid id;
        private Guid testID;
        private Guid userID;
        private string studentName;
        private string studentSurname;
        private int correctAnswers;
        private bool testCompleted;
        #endregion
        #region props
        [JsonProperty(nameof(ID))]
        public Guid ID { get => id; set => SetProperty(ref id, value); }

        [JsonProperty(nameof(TestID))]
        public Guid TestID { get => testID; set => SetProperty(ref testID, value); }

        [JsonProperty(nameof(UserID))]
        public Guid UserID { get => userID; set => SetProperty(ref userID, value); }

        [JsonProperty(nameof(StudentName))]
        public string StudentName { get => studentName; set => SetProperty(ref studentName, value); }

        [JsonProperty(nameof(StudentSurname))]
        public string StudentSurname { get => studentSurname; set => SetProperty(ref studentSurname, value); }

        [JsonProperty(nameof(CorrectAnswers))]
        public int CorrectAnswers { get => correctAnswers; set => SetProperty(ref correctAnswers, value); }

        [JsonProperty(nameof(TestCompleted))]
        public bool TestCompleted { get => testCompleted; set => SetProperty(ref testCompleted, value); }
        #endregion
    }
}
