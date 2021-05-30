using Newtonsoft.Json;
using System;

namespace ServerModels.Models
{
    public class RolesPermissionsModel
    {
        #region props
        [JsonProperty(nameof(Id))]
        public Guid Id { get; set; }

        [JsonProperty(nameof(IdRole))]
        public Guid IdRole { get; set; }

        [JsonProperty(nameof(IdPermission))]
        public Guid IdPermission { get; set; }
        #endregion
    }
}
