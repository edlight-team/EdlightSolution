using Newtonsoft.Json;
using System;

namespace ServerModels.Models
{
    public class UsersDialogsModel
    {
        #region props
        [JsonProperty(nameof(Id))]
        public Guid Id { get; set; }

        [JsonProperty(nameof(IdUser))]
        public Guid IdUser { get; set; }

        [JsonProperty(nameof(IdDialog))]
        public Guid IdDialog { get; set; }
        #endregion
    }
}
