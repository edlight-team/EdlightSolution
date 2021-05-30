using Newtonsoft.Json;
using System;

namespace ServerModels.Models
{
    public class StorageFilesModel
    {
        #region props
        [JsonProperty(nameof(ID))]
        public Guid ID { get; set; }

        [JsonProperty(nameof(StorageID))]
        public Guid StorageID { get; set; }

        [JsonProperty(nameof(StudentID))]
        public Guid StudentID { get; set; }

        [JsonProperty(nameof(FileName))]
        public string FileName { get; set; }
        #endregion
    }
}
