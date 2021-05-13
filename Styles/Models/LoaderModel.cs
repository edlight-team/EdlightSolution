using Prism.Mvvm;

namespace Styles.Models
{
    public sealed class LoaderModel : BindableBase
    {
        private bool isActive;
        private string message;

        public bool IsActive
        {
            get
            {
                RaisePropertyChanged(nameof(PanelIndex));
                return isActive;
            }
            set => SetProperty(ref isActive, value);
        }
        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }
        public int PanelIndex => isActive ? 100 : 0;

        public LoaderModel()
        {
            IsActive = false;
        }
        public LoaderModel(string message)
        {
            IsActive = true;
            Message = message;
        }
    }
}
