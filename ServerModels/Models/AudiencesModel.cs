using Newtonsoft.Json;
using System;

namespace ServerModels.Models
{
    public class AudiencesModel
    {
        #region props
        [JsonProperty(nameof(Id))]
        public Guid Id { get; set; }

        [JsonProperty(nameof(NumberAudience))]
        public string NumberAudience { get; set; }
        #endregion
    }
}
