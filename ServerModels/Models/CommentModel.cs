using Newtonsoft.Json;
using System;

namespace ServerModels.Models
{
    public class CommentModel
    {
        #region props

        [JsonProperty(nameof(Id))]
        public Guid Id { get; set; }

        [JsonProperty(nameof(IdLesson))]
        public Guid IdLesson { get; set; }

        [JsonProperty(nameof(IdUser))]
        public Guid IdUser { get; set; }

        [JsonProperty(nameof(Date))]
        public DateTime Date { get; set; }

        [JsonProperty(nameof(Message))]
        public string Message { get; set; }

        #endregion
    }
}
