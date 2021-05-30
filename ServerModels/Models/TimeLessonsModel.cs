using Newtonsoft.Json;
using System;

namespace ServerModels.Models
{
    public class TimeLessonsModel
    {
        #region props

        /// <summary>
        /// ИД
        /// </summary>
        [JsonProperty(nameof(Id))]
        public Guid Id { get; set; }

        /// <summary>
        /// Начало
        /// </summary>
        [JsonProperty(nameof(StartTime))]
        public string StartTime { get; set; }

        /// <summary>
        /// Конец
        /// </summary>
        [JsonProperty(nameof(EndTime))]
        public string EndTime { get; set; }

        /// <summary>
        /// Время перерыва
        /// </summary>
        [JsonProperty(nameof(BreakTime))]
        public string BreakTime { get; set; }

        #endregion
    }
}
