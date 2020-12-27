using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Prism.Mvvm;

namespace ApplicationModels
{
    class GroupModel : BindableBase
    {
        #region feilds
        private string id;
        private string title;
        private string curatorID;
        private string[] studentsID;
        #endregion
        #region props
        [BsonId]
        public string ID { get => id; set => SetProperty(ref id, value); }

        [BsonElement(nameof(Title))]
        [JsonProperty(nameof(Title))]
        public string Title { get => title; set => SetProperty(ref title, value); }

        [BsonElement(nameof(CuratorID))]
        [BsonIgnoreIfNull]
        [JsonProperty(nameof(CuratorID))]
        public string CuratorID { get => curatorID; set => SetProperty(ref curatorID, value); }

        [BsonElement(nameof(StudentsID))]
        [BsonIgnoreIfNull]
        [JsonProperty(nameof(StudentsID))]
        public string[] StudentsID { get => studentsID; set => SetProperty(ref studentsID, value); }
        #endregion
        #region ctor
        public GroupModel()
        {
            ID = ObjectId.GenerateNewId().ToString();
        }
        #endregion
    }
}
