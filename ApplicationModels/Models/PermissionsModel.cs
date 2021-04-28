using Newtonsoft.Json;
using Prism.Mvvm;
using System;

namespace ApplicationModels.Models
{
    public class PermissionsModel : BindableBase
    {
        #region fields

        private Guid id;
        private string permissionName;
        private string permissionDescription;

        #endregion
        #region props

        [JsonProperty(nameof(Id))]
        public Guid Id { get => id; set => SetProperty(ref id, value); }

        [JsonProperty(nameof(PermissionName))]
        public string PermissionName { get => permissionName ??= string.Empty; set => SetProperty(ref permissionName, value); }

        [JsonProperty(nameof(PermissionDescription))]
        public string PermissionDescription { get => permissionDescription ??= string.Empty; set => SetProperty(ref permissionDescription, value); }

        #endregion
    }
}
