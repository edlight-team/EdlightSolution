using Newtonsoft.Json;
using Prism.Mvvm;
using System;

namespace ApplicationModels.Models
{
    public class DialogsModel : BindableBase
    {
        #region fields

        private Guid id;
        private string titleDialog;

        #endregion
        #region props

        [JsonProperty(nameof(Id))]
        public Guid Id { get => id; set => SetProperty(ref id, value); }

        [JsonProperty(nameof(TitleDialog))]
        public string TitleDialog { get => titleDialog ??= string.Empty; set => SetProperty(ref titleDialog, value); }

        #endregion
    }
}
