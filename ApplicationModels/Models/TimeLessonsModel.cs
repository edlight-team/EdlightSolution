using Newtonsoft.Json;
using Prism.Mvvm;
using System;

namespace ApplicationModels.Models
{
    public class TimeLessonsModel : BindableBase
    {
        #region fields

        private Guid id;
        private string startTime;
        private string endTime;
        private string breakTime;

        #endregion
        #region props

        /// <summary>
        /// ИД
        /// </summary>
        [JsonProperty(nameof(Id))]
        public Guid Id { get => id; set => SetProperty(ref id, value); }

        /// <summary>
        /// Начало
        /// </summary>
        [JsonProperty(nameof(StartTime))]
        public string StartTime { get => startTime ??= string.Empty; set => SetProperty(ref startTime, value); }

        /// <summary>
        /// Конец
        /// </summary>
        [JsonProperty(nameof(EndTime))]
        public string EndTime { get => endTime ??= string.Empty; set => SetProperty(ref endTime, value); }

        /// <summary>
        /// Время перерыва
        /// </summary>
        [JsonProperty(nameof(BreakTime))]
        public string BreakTime { get => breakTime ??= string.Empty; set => SetProperty(ref breakTime, value); }

        #endregion
    }
}
