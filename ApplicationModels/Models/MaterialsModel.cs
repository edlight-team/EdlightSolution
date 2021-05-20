using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;

namespace ApplicationModels.Models
{
    public class MaterialsModel : BindableBase
    {
        #region fields

        private Guid id;
        private Guid idLesson;
        private Guid idUser;
        private string title;
        private string description;
        private string materialPath;

        private DelegateCommand<object> loadMaterialCommand;
        private DelegateCommand<object> deleteMaterialCommand;

        #endregion
        #region props

        [JsonProperty(nameof(Id))]
        public Guid Id { get => id; set => SetProperty(ref id, value); }

        [JsonProperty(nameof(IdLesson))]
        public Guid IdLesson { get => idLesson; set => SetProperty(ref idLesson, value); }

        [JsonProperty(nameof(IdUser))]
        public Guid IdUser { get => idUser; set => SetProperty(ref idUser, value); }

        [JsonProperty(nameof(Title))]
        public string Title { get => title ??= string.Empty; set => SetProperty(ref title, value); }

        [JsonProperty(nameof(Description))]
        public string Description { get => description ??= string.Empty; set => SetProperty(ref description, value); }

        [JsonProperty(nameof(MaterialPath))]
        public string MaterialPath { get => materialPath ??= string.Empty; set => SetProperty(ref materialPath, value); }

        [JsonIgnore]
        public DelegateCommand<object> LoadMaterialCommand { get => loadMaterialCommand; set => SetProperty(ref loadMaterialCommand, value); }
        [JsonIgnore]
        public DelegateCommand<object> DeleteMaterialCommand { get => deleteMaterialCommand; set => SetProperty(ref deleteMaterialCommand, value); }

        #endregion
    }
}
