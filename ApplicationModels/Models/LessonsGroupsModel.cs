using Newtonsoft.Json;
using Prism.Mvvm;
using System;

namespace ApplicationModels.Models
{
    public class LessonsGroupsModel : BindableBase
    {
        #region fields

        private Guid id;
        private Guid idLesson;
        private Guid idGroup;

        #endregion
        #region props

        [JsonProperty(nameof(Id))]
        public Guid Id { get => id; set => SetProperty(ref id, value); }

        [JsonProperty(nameof(IdLesson))]
        public Guid IdLesson { get => idLesson; set => SetProperty(ref idLesson, value); }

        [JsonProperty(nameof(IdGroup))]
        public Guid IdGroup { get => idGroup; set => SetProperty(ref idGroup, value); }

        #endregion
    }
}
