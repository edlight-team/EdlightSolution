using System.Collections.ObjectModel;
using System.ComponentModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Prism.Mvvm;

namespace ApplicationModels
{
    public class UserModel : BindableBase, IDataErrorInfo
    {
        #region fields
        private string id;
        private string login;
        private string password;
        #endregion
        #region props
        [BsonId]
        public string _id { get => id; set => SetProperty(ref id, value); }

        [BsonElement(nameof(Login))]
        [JsonProperty(nameof(Login))]
        public string Login
        {
            get
            {
                RaisePropertyChanged(nameof(Error));
                return login;
            }
            set => SetProperty(ref login, value); }

        [BsonElement(nameof(Password))]
        [JsonProperty(nameof(Password))]
        public string Password { get => password; set => SetProperty(ref password, value); }
        #endregion
        #region ctor
        public UserModel()
        {
            _id = ObjectId.GenerateNewId().ToString();
        } 
        #endregion
        #region errors

        //private ObservableCollection<string> errors;
        //[JsonIgnore]
        //[BsonIgnore]
        //public ObservableCollection<string> Errors
        //{
        //    get => errors ??= new();
        //    set => SetProperty(ref errors, value);
        //}

        [JsonIgnore]
        [BsonIgnore]
        public string Error { get; }
        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                switch (columnName)
                {
                    case nameof(Login):
                        if (Login.Length < 3)
                        {
                            error = "Логин должен состоять как минимум из 3 символов.";
                        }
                        break;
                    case nameof(Password):
                        if (Password.Length < 3)
                        {
                            error = "Пароль должен состоять как минимум из 3 символов.";
                        }
                        break;
                }
                return error;
            }
        } 
        #endregion
    }
}
