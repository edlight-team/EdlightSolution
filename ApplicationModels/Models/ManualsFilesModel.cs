using Newtonsoft.Json;
using Prism.Mvvm;
using System;

namespace ApplicationModels.Models
{
    public class ManualsFilesModel : BindableBase
    {
        #region fields
        private Guid id;
        private Guid creatorID;
        private Guid groupID;
        private string fileName;
        #endregion
        #region props
        [JsonProperty(nameof(ID))]
        public Guid ID { get => id; set => SetProperty(ref id, value); }

        [JsonProperty(nameof(CreatorID))]
        public Guid CreatorID { get => creatorID; set => SetProperty(ref creatorID, value); }

        [JsonProperty(nameof(GroupID))]
        public Guid GroupID { get => groupID; set => SetProperty(ref groupID, value); }

        [JsonProperty(nameof(FileName))]
        public string FileName { get => fileName; set => SetProperty(ref fileName, value); }
        #endregion
    }
}
