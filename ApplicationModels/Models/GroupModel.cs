using Newtonsoft.Json;
using Prism.Mvvm;

namespace ApplicationModels.Models
{
    public class GroupModel : BindableBase
    {
        #region feilds
        private string id;
        private string title;
        private string curatorID;
        private string[] studentsID;
        #endregion
        #region props
        public string _id { get => id; set => SetProperty(ref id, value); }

        [JsonProperty(nameof(Title))]
        public string Title { get => title; set => SetProperty(ref title, value); }

        [JsonProperty(nameof(CuratorID))]
        public string CuratorID { get => curatorID; set => SetProperty(ref curatorID, value); }

        [JsonProperty(nameof(StudentsID))]
        public string[] StudentsID { get => studentsID; set => SetProperty(ref studentsID, value); }
        #endregion
    }
}
