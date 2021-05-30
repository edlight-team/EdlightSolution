using Newtonsoft.Json;
using System;

namespace ServerModels.Models
{
    public class DialogsModel
    {
        #region props
        [JsonProperty(nameof(Id))]
        public Guid Id { get; set; }

        [JsonProperty(nameof(TitleDialog))]
        public string TitleDialog { get; set; }
        #endregion
    }
}
