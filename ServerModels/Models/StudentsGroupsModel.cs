using Newtonsoft.Json;
using System;

namespace ServerModels.Models
{
    public class StudentsGroupsModel
    {
        #region props
        [JsonProperty(nameof(Id))]
        public Guid Id { get; set; }

        [JsonProperty(nameof(IdStudent))]
        public Guid IdStudent { get; set; }

        [JsonProperty(nameof(IdGroup))]
        public Guid IdGroup { get; set; }
        #endregion
    }
}
