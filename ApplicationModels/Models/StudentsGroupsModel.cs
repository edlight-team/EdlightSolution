using Newtonsoft.Json;
using Prism.Mvvm;
using System;

namespace ApplicationModels.Models
{
    public class StudentsGroupsModel : BindableBase
    {
        #region fields

        private Guid id;
        private Guid idStudent;
        private Guid idGroup;

        #endregion
        #region props

        [JsonProperty(nameof(Id))]
        public Guid Id { get => id; set => SetProperty(ref id, value); }

        [JsonProperty(nameof(IdStudent))]
        public Guid IdStudent { get => idStudent; set => SetProperty(ref idStudent, value); }

        [JsonProperty(nameof(IdGroup))]
        public Guid IdGroup { get => idGroup; set => SetProperty(ref idGroup, value); }

        #endregion
    }
}
