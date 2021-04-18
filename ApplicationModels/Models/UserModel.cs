using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.ComponentModel;

namespace ApplicationModels.Models
{
    public class UserModel : BindableBase, IDataErrorInfo
    {
        #region fields
        private Guid id;
        private string login;
        private string password;
        private string name;
        private string surname;
        private string patronymic;
        private int sex;
        private int age;
        private string[] role;

        #endregion
        #region props
        [JsonProperty(nameof(ID))]
        public Guid ID { get => id; set => SetProperty(ref id, value); }

        [JsonProperty(nameof(Login))]
        public string Login { get => login; set => SetProperty(ref login, value); }

        [JsonProperty(nameof(Password))]
        public string Password { get => password; set => SetProperty(ref password, value); }

        [JsonProperty(nameof(Name))]
        public string Name { get => name; set => SetProperty(ref name, value); }

        [JsonProperty(nameof(Surname))]
        public string Surname { get => surname; set => SetProperty(ref surname, value); }

        [JsonProperty(nameof(Patrnymic))]
        public string Patrnymic { get => patronymic; set => SetProperty(ref patronymic, value); }

        [JsonProperty(nameof(Sex))]
        public int Sex { get => sex; set => SetProperty(ref sex, value); }

        [JsonProperty(nameof(Age))]
        public int Age { get => age; set => SetProperty(ref age, value); }

        [JsonProperty(nameof(Role))]
        public string[] Role { get => role; set => SetProperty(ref role, value); }
        #endregion
        #region errors

        [JsonIgnore]
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
