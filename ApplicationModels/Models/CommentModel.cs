using Newtonsoft.Json;
using Prism.Mvvm;
using System;

namespace ApplicationModels.Models
{
    public class CommentModel : BindableBase
    {
        #region fields

        private Guid id;
        private Guid idLesson;
        private Guid idUser;
        private UserModel user;
        private DateTime date;
        private string message;

        #endregion
        #region props

        [JsonProperty(nameof(Id))]
        public Guid Id { get => id; set => SetProperty(ref id, value); }

        [JsonProperty(nameof(IdLesson))]
        public Guid IdLesson { get => idLesson; set => SetProperty(ref idLesson, value); }

        [JsonProperty(nameof(IdUser))]
        public Guid IdUser { get => idUser; set => SetProperty(ref idUser, value); }

        [JsonIgnore]
        public UserModel User { get => user; set => SetProperty(ref user, value); }

        [JsonProperty(nameof(Date))]
        public DateTime Date { get => date; set => SetProperty(ref date, value); }

        [JsonProperty(nameof(Message))]
        public string Message { get => message ??= string.Empty; set => SetProperty(ref message, value); }

        #endregion
    }
}
