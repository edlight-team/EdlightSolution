using Newtonsoft.Json;
using Prism.Mvvm;
using System;

namespace ApplicationModels.Models
{
    public class StorageFilesModel : BindableBase
    {
        #region fields
        private Guid id;
        private Guid storageID;
        private Guid studentID;
        private string fileName;
        #endregion
        #region props
        [JsonProperty(nameof(ID))]
        public Guid ID { get => id; set => SetProperty(ref id, value); }

        [JsonProperty(nameof(StorageID))]
        public Guid StorageID { get => storageID; set => SetProperty(ref storageID, value); }

        [JsonProperty(nameof(StudentID))]
        public Guid StudentID { get => studentID; set => SetProperty(ref studentID, value); }

        [JsonProperty(nameof(FileName))]
        public string FileName { get => fileName; set => SetProperty(ref fileName, value); }
        #endregion
    }
}
