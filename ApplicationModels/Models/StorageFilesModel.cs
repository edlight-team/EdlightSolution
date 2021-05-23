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

        private string studentFullName;
        private bool fileAdded;
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

        public string StudentFullName { get => studentFullName; set => SetProperty(ref studentFullName, value); }
        public bool FileAdded { get => !string.IsNullOrEmpty(FileName); private set { } }
        #endregion
    }
}
