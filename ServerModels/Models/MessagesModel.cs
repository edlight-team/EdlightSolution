using Newtonsoft.Json;
using System;

namespace ServerModels.Models
{
    public class MessagesModel
    {
        #region props
        [JsonProperty(nameof(Id))]
        public Guid Id { get; set; }

        [JsonProperty(nameof(IdDialog))]
        public Guid IdDialog { get; set; }

        [JsonProperty(nameof(IdUserSender))]
        public Guid IdUserSender { get; set; }

        [JsonProperty(nameof(TextMessage))]
        public string TextMessage { get; set; }

        [JsonProperty(nameof(IsRead))]
        public bool IsRead { get; set; }

        [JsonProperty(nameof(SendingTime))]
        public DateTime SendingTime { get; set; }
        #endregion
    }
}
