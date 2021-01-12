using System.ComponentModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Prism.Mvvm;

namespace ApplicationModels.Models
{
    public class UserModel : BindableBase, IDataErrorInfo
    {
        #region fields
        private string id;
        private string login;
        private string password;
        private string name;
        private string surname;
        private string patronymic;
        private string sex;
        private int age;
        private string[] role;

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
            set => SetProperty(ref login, value);
        }

        [BsonElement(nameof(Password))]
        [JsonProperty(nameof(Password))]
        public string Password { get => password; set => SetProperty(ref password, value); }

        [BsonElement(nameof(Name))]
        [JsonProperty(nameof(Name))]
        public string Name { get => name; set => SetProperty(ref name, value); }

        [BsonElement(nameof(Surname))]
        [JsonProperty(nameof(Surname))]
        public string Surname { get => surname; set => SetProperty(ref surname, value); }

        [BsonElement(nameof(Patrnymic))]
        [BsonIgnoreIfNull]
        [JsonProperty(nameof(Patrnymic))]
        public string Patrnymic { get => patronymic; set => SetProperty(ref patronymic, value); }

        [BsonElement(nameof(Sex))]
        [JsonProperty(nameof(Sex))]
        public string Sex { get => sex; set => SetProperty(ref sex, value); }

        [BsonElement(nameof(Age))]
        [JsonProperty(nameof(Age))]
        public int Age { get => age; set => SetProperty(ref age, value); }

        [BsonElement(nameof(Role))]
        [BsonIgnoreIfNull]
        [JsonProperty(nameof(Role))]
        public string[] Role { get => role; set => SetProperty(ref role, value); }
        #endregion
        #region ctor
        public UserModel() => _id = ObjectId.GenerateNewId().ToString();
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
