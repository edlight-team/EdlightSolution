using Prism.Mvvm;
using System;
using System.Threading.Tasks;

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
        [Obsolete("Use .SetDefaultLoadingInfo() and .Clear()")]
        public LoaderModel(string message)
        {
            IsActive = true;
            Message = message;
        }
    }
    public static class LoaderExtension
    {
        public static void SetDefaultLoadingInfo(this LoaderModel loader)
        {
            loader.IsActive = true;
            loader.Message = "Выполняется загрузка";
        }
        public static void SetCountLoadingInfo(this LoaderModel loader, int fromCount, int toCount)
        {
            loader.IsActive = true;
            loader.Message = $"Обработка {fromCount} / {toCount}";
        }
        public static async Task Clear(this LoaderModel loader)
        {
            //await Task.Delay(1000);
            await Task.Delay(100);
            loader.IsActive = false;
            loader.Message = null;
        }
    }
}
