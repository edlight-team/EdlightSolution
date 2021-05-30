using Newtonsoft.Json;
using System;

namespace ServerModels.Models
{
    public class TestResultsModel
    {
        #region props
        [JsonProperty(nameof(ID))]
        public Guid ID { get; set; }

        [JsonProperty(nameof(TestID))]
        public Guid TestID { get; set; }

        [JsonProperty(nameof(UserID))]
        public Guid UserID { get; set; }

        [JsonProperty(nameof(StudentName))]
        public string StudentName { get; set; }

        [JsonProperty(nameof(StudentSurname))]
        public string StudentSurname { get; set; }

        [JsonProperty(nameof(CorrectAnswers))]
        public int CorrectAnswers { get; set; }

        [JsonProperty(nameof(TestCompleted))]
        public bool TestCompleted { get; set; }
        #endregion
    }
}
