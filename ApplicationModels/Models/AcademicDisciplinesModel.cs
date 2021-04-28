using Newtonsoft.Json;
using Prism.Mvvm;
using System;

namespace ApplicationModels.Models
{
    public class AcademicDisciplinesModel : BindableBase
    {
        #region fields

        private Guid id;
        private string title;

        #endregion
        #region props

        [JsonProperty(nameof(Id))]
        public Guid Id { get => id; set => SetProperty(ref id, value); }

        [JsonProperty(nameof(Title))]
        public string Title { get => title; set => SetProperty(ref title, value); }

        #endregion
    }
}
