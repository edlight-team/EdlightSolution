using Newtonsoft.Json;
using Prism.Mvvm;
using System;

namespace ApplicationModels.Models
{
    public class TimeLessonsModel : BindableBase
    {
        #region fields

        private Guid id;
        private string classNumber;
        private string startClass;
        private string startBreak;
        private string endBreak;
        private string endClass;

        #endregion
        #region props

        [JsonProperty(nameof(Id))]
        public Guid Id { get => id; set => SetProperty(ref id, value); }

        [JsonProperty(nameof(ClassNumber))]
        public string ClassNumber { get => classNumber; set => SetProperty(ref classNumber, value); }

        [JsonProperty(nameof(StartClass))]
        public string StartClass { get => startClass; set => SetProperty(ref startClass, value); }

        [JsonProperty(nameof(StartBreak))]
        public string StartBreak { get => startBreak; set => SetProperty(ref startBreak, value); }

        [JsonProperty(nameof(EndBreak))]
        public string EndBreak { get => endBreak; set => SetProperty(ref endBreak, value); }

        [JsonProperty(nameof(EndClass))]
        public string EndClass { get => endClass; set => SetProperty(ref endClass, value); }

        #endregion
    }
}
