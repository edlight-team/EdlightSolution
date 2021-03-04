using Newtonsoft.Json;
using Prism.Mvvm;

namespace ApplicationModels.Models
{
    public class RoleModel : BindableBase
    {
        #region fields
        private string id;
        private string title;
        #endregion
        #region props
        public string _id { get => id; set => SetProperty(ref id, value); }

        [JsonProperty(nameof(Title))]
        public string Title { get => title; set => SetProperty(ref title, value); }
        #endregion
    }
}
