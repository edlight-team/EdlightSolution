using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Prism.Mvvm;

namespace ApplicationModels
{
    class RoleModel : BindableBase
    {
        #region fields
        private string id;
        private string title;
        private string[] usersID;
        #endregion
        #region props
        [BsonId]
        public string ID { get => id; set => SetProperty(ref id, value); }

        [BsonElement(nameof(Title))]
        [JsonProperty(nameof(Title))]
        public string Title { get => title; set => SetProperty(ref title, value); }

        [BsonElement(nameof(UsersID))]
        [BsonIgnoreIfNull]
        [JsonProperty(nameof(UsersID))]
        public string[] UsersID { get => usersID; set => SetProperty(ref usersID, value); }
        #endregion
        #region ctor
        public RoleModel()
        {
            ID = ObjectId.GenerateNewId().ToString();
        }
        #endregion
    }
}
