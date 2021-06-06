using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;

namespace ApplicationModels.Models
{
    public class LearnPlanesModel : BindableBase
    {
        private Guid id;
        private string name;
        private string path;

        [JsonProperty(nameof(Id))]
        public Guid Id { get => id; set => SetProperty(ref id, value); }

        [JsonProperty(nameof(Name))]
        public string Name { get => name ??= string.Empty; set => SetProperty(ref name, value); }

        [JsonProperty(nameof(Path))]
        public string Path { get => path ??= string.Empty; set => SetProperty(ref path, value); }

        private DelegateCommand<object> _openPlanCommand;
        private DelegateCommand<object> _deletePlanCommand;

        [JsonIgnore]
        public DelegateCommand<object> OpenPlanCommand { get => _openPlanCommand; set => SetProperty(ref _openPlanCommand, value); }

        [JsonIgnore]
        public DelegateCommand<object> DeletePlanCommand { get => _deletePlanCommand; set => SetProperty(ref _deletePlanCommand, value); }
    }
}
