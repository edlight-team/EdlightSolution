using Newtonsoft.Json;
using System;

namespace ServerModels.Models
{
    public class ManualsFilesModel
    {
        #region props
        [JsonProperty(nameof(ID))]
        public Guid ID { get; set; }

        [JsonProperty(nameof(CreatorID))]
        public Guid CreatorID { get; set; }

        [JsonProperty(nameof(GroupID))]
        public Guid GroupID { get; set; }

        [JsonProperty(nameof(FileName))]
        public string FileName { get; set; }
        #endregion
    }
}
