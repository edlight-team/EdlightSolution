using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Styles.Models;

namespace EdlightDesktopClient.ViewModels.Dictionaries
{
    [RegionMemberLifetime(KeepAlive = false)]
    public class EditDisciplinesViewModel : BindableBase
    {
        #region services

        private readonly IRegionManager manager;

        #endregion
        #region fields

        private LoaderModel _loader;
        private string _saveButtonText;

        #endregion
        #region props

        public LoaderModel Loader { get => _loader; set => SetProperty(ref _loader, value); }
        public string SaveButtonText { get => _saveButtonText; set => SetProperty(ref _saveButtonText, value); }

        #endregion
        #region errors

        private bool _hasErrors;
        private string _timeToSmallError;

        public bool HasErrors { get => _hasErrors; set => SetProperty(ref _hasErrors, value); }
        public string Errors { get => _timeToSmallError ??= string.Empty; set => SetProperty(ref _timeToSmallError, value); }

        #endregion
        #region commands

        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand CloseModalCommand { get; private set; }

        #endregion
        #region ctor

        public EditDisciplinesViewModel(IRegionManager manager)
        {
            Loader = new();
            this.manager = manager;

            LoadedCommand = new DelegateCommand(OnLoaded);
            CloseModalCommand = new DelegateCommand(OnCloseModal);
        }
        private void OnLoaded()
        {

        }
        #endregion
        #region methods
        private void OnCloseModal() => manager.Regions[BaseMethods.RegionNames.ModalRegion].RemoveAll();

        #endregion
    }
}
