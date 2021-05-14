using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationModels.Models
{
    public class TestsModel:BindableBase
    {
        #region fields
        private Guid id;
        private string questions;
        #endregion
        #region props
        [JsonProperty(nameof(ID))]
        public Guid ID { get => id; set => SetProperty(ref id, value); }

        [JsonProperty(nameof(Questions))]
        public string Questions { get => questions ??= string.Empty; set => SetProperty(ref questions, value); }
        #endregion
    }
}
