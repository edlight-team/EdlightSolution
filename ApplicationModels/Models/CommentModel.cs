using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Windows;

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
        #region commands
        private Visibility contextMenuVisibility;
        private DelegateCommand<object> deleteCommentCommand;

        [JsonIgnore]
        public Visibility ContextMenuVisibility { get => contextMenuVisibility; set => SetProperty(ref contextMenuVisibility, value); }
        [JsonIgnore]
        public DelegateCommand<object> DeleteCommentCommand { get => deleteCommentCommand; set => SetProperty(ref deleteCommentCommand, value); }
        #endregion
    }
}
