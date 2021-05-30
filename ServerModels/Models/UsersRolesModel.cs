using Newtonsoft.Json;
using System;

namespace ServerModels.Models
{
    public class UsersRolesModel
    {
        #region props
        [JsonProperty(nameof(Id))]
        public Guid Id { get; set; }

        [JsonProperty(nameof(IdUser))]
        public Guid IdUser { get; set; }

        [JsonProperty(nameof(IdRole))]
        public Guid IdRole { get; set; }
        #endregion
    }
}
