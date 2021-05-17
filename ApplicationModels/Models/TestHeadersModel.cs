using Newtonsoft.Json;
using Prism.Mvvm;
using System;

namespace ApplicationModels.Models
{
    public class TestHeadersModel : BindableBase
    {
        #region fields
        public Guid id;
        private Guid testID;
        private Guid groupID;
        private Guid teacherID;
        private string testName;
        private string testType;
        private string testTime;
        private int countQuestions;
        #endregion
        #region props
        [JsonProperty(nameof(ID))]
        public Guid ID { get => id; set => SetProperty(ref id, value); }

        [JsonProperty(nameof(TestID))]
        public Guid TestID { get => testID; set => SetProperty(ref testID, value); }

        [JsonProperty(nameof(GroupID))]
        public Guid GroupID { get => groupID; set => SetProperty(ref groupID, value); }

        [JsonProperty(nameof(TeacherID))]
        public Guid TeacherID { get => teacherID; set => SetProperty(ref teacherID, value); }

        [JsonProperty(nameof(TestName))]
        public string TestName { get => testName ??= string.Empty; set => SetProperty(ref testName, value); }

        [JsonProperty(nameof(TestType))]
        public string TestType { get => testType ??= string.Empty; set => SetProperty(ref testType, value); }

        [JsonProperty(nameof(TestTime))]
        public string TestTime { get => testTime ??= string.Empty; set => SetProperty(ref testTime, value); }

        [JsonProperty(nameof(CountQuestions))]
        public int CountQuestions { get => countQuestions; set => SetProperty(ref countQuestions, value); }
        #endregion
    }
}
