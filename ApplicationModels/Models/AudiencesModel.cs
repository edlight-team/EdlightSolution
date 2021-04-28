using Newtonsoft.Json;
using Prism.Mvvm;
using System;

namespace ApplicationModels.Models
{
    public class AudiencesModel : BindableBase
    {
        #region fields

        private Guid id;
        private string numberAudience;

        #endregion
        #region props

        [JsonProperty(nameof(Id))]
        public Guid Id { get => id; set => SetProperty(ref id, value); }

        [JsonProperty(nameof(NumberAudience))]
        public string NumberAudience { get => numberAudience ??= string.Empty; set => SetProperty(ref numberAudience, value); }

        #endregion
    }
}
