using Newtonsoft.Json;
using System;

namespace ServerModels.Models
{
    public class LearnPlanesModel
    {
        /// <summary>
        /// Ид учебного плана
        /// </summary>
        [JsonProperty(nameof(Id))]
        public Guid Id { get; set; }

        /// <summary>
        /// Имя в файле нагрузки
        /// </summary>
        [JsonProperty(nameof(Name))]
        public string Name { get; set; }

        /// <summary>
        /// Путь на сервере
        /// </summary>
        [JsonProperty(nameof(Path))]
        public string Path { get; set; }
    }
}
