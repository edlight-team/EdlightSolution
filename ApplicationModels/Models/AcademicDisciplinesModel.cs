using Newtonsoft.Json;
using Prism.Commands;
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
        public string Title { get => title ??= string.Empty; set => SetProperty(ref title, value); }

        #endregion
        #region commands

        private DelegateCommand<object> editCommand;
        private DelegateCommand<object> deleteCommand;

        [JsonIgnore]
        public DelegateCommand<object> EditCommand { get => editCommand; set => SetProperty(ref editCommand, value); }
        [JsonIgnore]
        public DelegateCommand<object> DeleteCommand { get => deleteCommand; set => SetProperty(ref deleteCommand, value); }

        #endregion
    }
}
