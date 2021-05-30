using Newtonsoft.Json;
using System;

namespace ServerModels.Models
{
    public class TypeClassesModel
    {
        #region props
        [JsonProperty(nameof(Id))]
        public Guid Id { get; set; }

        [JsonProperty(nameof(Title))]
        public string Title { get; set; }

        [JsonProperty(nameof(ShortTitle))]
        public string ShortTitle { get; set; }

        [JsonProperty(nameof(ColorHex))]
        public string ColorHex { get; set; }
        #endregion
    }
}
