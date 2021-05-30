using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
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
        private List<int> daysPriority;

        #endregion
        #region props

        [JsonProperty(nameof(ID))]
        public Guid ID { get => id; set => SetProperty(ref id, value); }

        [JsonProperty(nameof(Login))]
        public string Login { get => login ??= string.Empty; set => SetProperty(ref login, value); }

        [JsonProperty(nameof(Password))]
        public string Password { get => password ??= string.Empty; set => SetProperty(ref password, value); }

        [JsonProperty(nameof(Name))]
        public string Name { get => name ??= string.Empty; set => SetProperty(ref name, value); }

        [JsonProperty(nameof(Surname))]
        public string Surname { get => surname ??= string.Empty; set => SetProperty(ref surname, value); }

        [JsonProperty(nameof(Patrnymic))]
        public string Patrnymic { get => patronymic ??= string.Empty; set => SetProperty(ref patronymic, value); }

        [JsonProperty(nameof(Sex))]
        public int Sex { get => sex; set => SetProperty(ref sex, value); }

        [JsonProperty(nameof(Age))]
        public int Age { get => age; set => SetProperty(ref age, value); }

        [JsonProperty(nameof(DaysPriority))]
        public List<int> DaysPriority { get => daysPriority; set => SetProperty(ref daysPriority, value); }

        #endregion
        #region errors

        [JsonIgnore]
        public string Error { get; }

        [JsonIgnore]
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
        #region methods

        [JsonIgnore]
        public string FullName { get => $"{Surname} {Name} {Patrnymic}"; }

        [JsonIgnore]
        public string Initials { get => $"{Surname} {Name[0]}.{Patrnymic[0]}."; }

        #endregion
        #region commands

        private DelegateCommand<object> editCommand;
        private DelegateCommand<object> deleteCommand;

        [JsonIgnore]
        public DelegateCommand<object> EditCommand { get => editCommand; set => SetProperty(ref editCommand, value); }
        [JsonIgnore]
        public DelegateCommand<object> DeleteCommand { get => deleteCommand; set => SetProperty(ref deleteCommand, value); }

        #endregion
    }
}
