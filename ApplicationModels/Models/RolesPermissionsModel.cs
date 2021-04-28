using Newtonsoft.Json;
using Prism.Mvvm;
using System;

namespace ApplicationModels.Models
{
    public class RolesPermissionsModel : BindableBase
    {
        #region fields

        private Guid id;
        private Guid idRole;
        private Guid idPermission;

        #endregion
        #region props

        [JsonProperty(nameof(Id))]
        public Guid Id { get => id; set => SetProperty(ref id, value); }

        [JsonProperty(nameof(IdRole))]
        public Guid IdRole { get => idRole; set => SetProperty(ref idRole, value); }

        [JsonProperty(nameof(IdPermission))]
        public Guid IdPermission { get => idPermission; set => SetProperty(ref idPermission, value); }

        #endregion
    }
}
