using Newtonsoft.Json;
using Prism.Mvvm;
using System;

namespace ApplicationModels.Models
{
    public class RolesModel : BindableBase
    {
        #region fields

        private Guid id;
        private string roleName;
        private string roleDescription;

        #endregion
        #region props

        [JsonProperty(nameof(Id))]
        public Guid Id { get => id; set => SetProperty(ref id, value); }

        [JsonProperty(nameof(RoleName))]
        public string RoleName { get => roleName; set => SetProperty(ref roleName, value); }

        [JsonProperty(nameof(RoleDescription))]
        public string RoleDescription { get => roleDescription; set => SetProperty(ref roleDescription, value); }

        #endregion
    }
}
