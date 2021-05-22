using Newtonsoft.Json;
using Prism.Mvvm;
using System;

namespace ApplicationModels.Models
{
    public class StoragesHeadersModel : BindableBase
    {
        #region field
        private Guid id;
        private Guid creatorID;
        private Guid groupID;
        private string storageName;
        private string dateCloseStorage;
        #endregion
        #region props
        [JsonProperty(nameof(ID))]
        public Guid ID { get => id; set => SetProperty(ref id, value); }

        [JsonProperty(nameof(CreatorID))]
        public Guid CreatorID { get => creatorID; set => SetProperty(ref creatorID, value); }

        [JsonProperty(nameof(GroupID))]
        public Guid GroupID { get => groupID; set => SetProperty(ref groupID, value); }

        [JsonProperty(nameof(StorageName))]
        public string StorageName { get => storageName; set => SetProperty(ref storageName, value); }

        [JsonProperty(nameof(DateCloseStorage))]
        public string DateCloseStorage { get => dateCloseStorage; set => SetProperty(ref dateCloseStorage, value); }
        #endregion
    }
}
