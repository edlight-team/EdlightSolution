using System;
using System.Runtime.Serialization;

namespace DatabaseModels
{
    [DataContract]
    /// <summary>
    /// Сущность пользователь
    /// </summary>
    public class UserModel
    {
        [DataMember]
        /// <summary>
        /// ИД пользователя
        /// </summary>
        public Guid ID { get; set; }

        [DataMember]
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login { get; set; }

        [DataMember]
        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password { get; set; }
    }
}
