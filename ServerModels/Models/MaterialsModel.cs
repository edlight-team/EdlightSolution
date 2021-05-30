using Newtonsoft.Json;
using System;

namespace ServerModels.Models
{
    public class MaterialsModel
    {
        #region props

        [JsonProperty(nameof(Id))]
        public Guid Id { get; set; }

        [JsonProperty(nameof(IdLesson))]
        public Guid IdLesson { get; set; }

        [JsonProperty(nameof(IdUser))]
        public Guid IdUser { get; set; }

        [JsonProperty(nameof(Title))]
        public string Title { get; set; }

        [JsonProperty(nameof(Description))]
        public string Description { get; set; }

        [JsonProperty(nameof(MaterialPath))]
        public string MaterialPath { get; set; }

        #endregion
    }
}
