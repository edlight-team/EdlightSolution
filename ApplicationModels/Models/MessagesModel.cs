using Newtonsoft.Json;
using Prism.Mvvm;
using System;

namespace ApplicationModels.Models
{
    public class MessagesModel : BindableBase
    {
        #region fields

        private Guid id;
        private Guid idDialog;
        private Guid idUserSender;
        private string textMessage;
        private bool isRead;
        private DateTime sendingTime;

        #endregion
        #region props

        [JsonProperty(nameof(Id))]
        public Guid Id { get => id; set => SetProperty(ref id, value); }

        [JsonProperty(nameof(IdDialog))]
        public Guid IdDialog { get => idDialog; set => SetProperty(ref idDialog, value); }

        [JsonProperty(nameof(IdUserSender))]
        public Guid IdUserSender { get => idUserSender; set => SetProperty(ref idUserSender, value); }

        [JsonProperty(nameof(TextMessage))]
        public string TextMessage { get => textMessage ??= string.Empty; set => SetProperty(ref textMessage, value); }

        [JsonProperty(nameof(IsRead))]
        public bool IsRead { get => isRead; set => SetProperty(ref isRead, value); }

        [JsonProperty(nameof(SendingTime))]
        public DateTime SendingTime { get => sendingTime; set => SetProperty(ref sendingTime, value); }

        #endregion
    }
}
