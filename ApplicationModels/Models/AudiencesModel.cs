using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;

namespace ApplicationModels.Models
{
    public class AudiencesModel : BindableBase
    {
        #region fields

        private Guid id;
        private string numberAudience;

        #endregion
        #region props

        [JsonProperty(nameof(Id))]
        public Guid Id { get => id; set => SetProperty(ref id, value); }

        [JsonProperty(nameof(NumberAudience))]
        public string NumberAudience { get => numberAudience ??= string.Empty; set => SetProperty(ref numberAudience, value); }

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
