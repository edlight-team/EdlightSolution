using Newtonsoft.Json;
using Prism.Mvvm;
using System;

namespace ApplicationModels.Models
{
    public class GroupsModel : BindableBase
    {
        #region fields

        private Guid id;
        private string group;

        #endregion
        #region props

        [JsonProperty(nameof(Id))]
        public Guid Id { get => id; set => SetProperty(ref id, value); }

        [JsonProperty(nameof(Group))]
        public string Group { get => group; set => SetProperty(ref group, value); }

        #endregion
    }
}
