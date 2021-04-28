using Newtonsoft.Json;
using Prism.Mvvm;
using System;

namespace ApplicationModels.Models
{
    public class UsersDialogsModel : BindableBase
    {
        #region fields

        private Guid id;
        private Guid idUser;
        private Guid idDialog;

        #endregion
        #region props

        [JsonProperty(nameof(Id))]
        public Guid Id { get => id; set => SetProperty(ref id, value); }

        [JsonProperty(nameof(IdUser))]
        public Guid IdUser { get => idUser; set => SetProperty(ref idUser, value); }

        [JsonProperty(nameof(IdDialog))]
        public Guid IdDialog { get => idDialog; set => SetProperty(ref idDialog, value); }

        #endregion
    }
}
