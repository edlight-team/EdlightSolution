using Newtonsoft.Json;
using Prism.Mvvm;
using System;

namespace ApplicationModels.Models
{
    public class WeekDaysModel : BindableBase
    {
        #region fields

        private Guid id;
        private DateTime day;

        #endregion
        #region props

        [JsonProperty(nameof(Id))]
        public Guid Id { get => id; set => SetProperty(ref id, value); }

        [JsonProperty(nameof(Day))]
        public DateTime Day { get => day; set => SetProperty(ref day, value); }

        #endregion
    }
}
