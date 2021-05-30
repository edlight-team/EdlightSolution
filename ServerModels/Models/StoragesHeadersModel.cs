using Newtonsoft.Json;
using System;

namespace ServerModels.Models
{
    public class StoragesHeadersModel
    {
        #region props
        [JsonProperty(nameof(ID))]
        public Guid ID { get; set; }

        [JsonProperty(nameof(CreatorID))]
        public Guid CreatorID { get; set; }

        [JsonProperty(nameof(GroupID))]
        public Guid GroupID { get; set; }

        [JsonProperty(nameof(StorageName))]
        public string StorageName { get; set; }

        [JsonProperty(nameof(DateCloseStorage))]
        public string DateCloseStorage { get; set; }
        #endregion
    }
}
