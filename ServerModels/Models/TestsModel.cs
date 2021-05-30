using Newtonsoft.Json;
using System;

namespace ServerModels.Models
{
    public class TestsModel
    {
        #region props
        [JsonProperty(nameof(ID))]
        public Guid ID { get; set; }

        [JsonProperty(nameof(Questions))]
        public string Questions { get; set; }
        #endregion
    }
}
