using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Windows.Media;

namespace ApplicationModels.Models
{
    public class TypeClassesModel : BindableBase
    {
        #region fields

        private Guid id;
        private string title;
        private string shortTitle;
        private string colorHex;
        private SolidColorBrush brush;

        #endregion
        #region props

        [JsonProperty(nameof(Id))]
        public Guid Id { get => id; set => SetProperty(ref id, value); }

        [JsonProperty(nameof(Title))]
        public string Title { get => title ??= string.Empty; set => SetProperty(ref title, value); }

        [JsonProperty(nameof(ShortTitle))]
        public string ShortTitle { get => shortTitle ??= string.Empty; set => SetProperty(ref shortTitle, value); }

        [JsonProperty(nameof(ColorHex))]
        public string ColorHex
        {
            get => colorHex ??= string.Empty;
            set
            {
                colorHex = value;
                Brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(value));
                RaisePropertyChanged(nameof(ColorHex));
            }
        }

        [JsonIgnore]
        public SolidColorBrush Brush { get => brush; set => SetProperty(ref brush, value); }

        #endregion
    }
}
