using Newtonsoft.Json;
using Prism.Mvvm;
using System;

namespace ApplicationModels.Models
{
    public class UsersRolesModel : BindableBase
    {
        #region fields

        private Guid id;
        private Guid idUser;
        private Guid idRole;

        #endregion
        #region props

        [JsonProperty(nameof(Id))]
        public Guid Id { get => id; set => SetProperty(ref id, value); }

        [JsonProperty(nameof(IdUser))]
        public Guid IdUser { get => idUser; set => SetProperty(ref idUser, value); }

        [JsonProperty(nameof(IdRole))]
        public Guid IdRole { get => idRole; set => SetProperty(ref idRole, value); }

        #endregion
    }
}
