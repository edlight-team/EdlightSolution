using Newtonsoft.Json;
using System;

namespace ServerModels.Models
{
    public class PermissionsModel
    {
        #region props
        [JsonProperty(nameof(Id))]
        public Guid Id { get; set; }

        [JsonProperty(nameof(PermissionName))]
        public string PermissionName { get; set; }

        [JsonProperty(nameof(PermissionDescription))]
        public string PermissionDescription { get; set; }
        #endregion
    }
}
