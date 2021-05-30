using Newtonsoft.Json;
using System;

namespace ServerModels.Models
{
    public class AcademicDisciplinesModel
    {
        #region props
        [JsonProperty(nameof(Id))]
        public Guid Id { get; set; }

        [JsonProperty(nameof(Title))]
        public string Title { get; set; }

        [JsonProperty(nameof(IdPriorityAudience))]
        public Guid IdPriorityAudience { get; set; }
        #endregion
    }
}
