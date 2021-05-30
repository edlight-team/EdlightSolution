using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ServerModels.Models
{
    public class UserModel
    {
        #region props
        [JsonProperty(nameof(ID))]
        public Guid ID { get; set; }

        [JsonProperty(nameof(Login))]
        public string Login { get; set; }

        [JsonProperty(nameof(Password))]
        public string Password { get; set; }

        [JsonProperty(nameof(Name))]
        public string Name { get; set; }

        [JsonProperty(nameof(Surname))]
        public string Surname { get; set; }

        [JsonProperty(nameof(Patrnymic))]
        public string Patrnymic { get; set; }

        [JsonProperty(nameof(Sex))]
        public int Sex { get; set; }

        [JsonProperty(nameof(Age))]
        public int Age { get; set; }

        [JsonProperty(nameof(DaysPriority))]
        public List<int> DaysPriority { get; set; }
        #endregion
    }
}
