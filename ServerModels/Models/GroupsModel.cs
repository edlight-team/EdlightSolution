using Newtonsoft.Json;
using System;

namespace ServerModels.Models
{
    public class GroupsModel
    {
        #region props
        [JsonProperty(nameof(Id))]
        public Guid Id { get; set; }

        [JsonProperty(nameof(Group))]
        public string Group { get; set; }
        #endregion
    }
}
