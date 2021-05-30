using Newtonsoft.Json;
using System;

namespace ServerModels.Models
{
    public class TestHeadersModel
    {
        #region props
        [JsonProperty(nameof(ID))]
        public Guid ID { get; set; }

        [JsonProperty(nameof(TestID))]
        public Guid TestID { get; set; }

        [JsonProperty(nameof(GroupID))]
        public Guid GroupID { get; set; }

        [JsonProperty(nameof(TeacherID))]
        public Guid TeacherID { get; set; }

        [JsonProperty(nameof(TestName))]
        public string TestName { get; set; }

        [JsonProperty(nameof(TestType))]
        public string TestType { get; set; }

        [JsonProperty(nameof(TestTime))]
        public string TestTime { get; set; }

        [JsonProperty(nameof(CountQuestions))]
        public int CountQuestions { get; set; }

        [JsonProperty(nameof(TestStartDate))]
        public string TestStartDate { get; set; }

        [JsonProperty(nameof(TestEndDate))]
        public string TestEndDate { get; set; }

        [JsonProperty(nameof(CountPoints))]
        public int CountPoints { get; set; }
        #endregion
    }
}
