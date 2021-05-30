using Newtonsoft.Json;
using System;

namespace ServerModels.Models
{
    public class RolesModel
    {
        #region props
        [JsonProperty(nameof(Id))]
        public Guid Id { get; set; }

        [JsonProperty(nameof(RoleName))]
        public string RoleName { get; set; }

        [JsonProperty(nameof(RoleDescription))]
        public string RoleDescription { get; set; }
        #endregion
    }
}
