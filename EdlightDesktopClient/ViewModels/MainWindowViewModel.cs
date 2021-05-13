using ApplicationWPFServices.MemoryService;
using EdlightDesktopClient.BaseMethods;
using Prism.Commands;
using Prism.Mvvm;
using Styles.Models;
using System.Windows;

namespace EdlightDesktopClient.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region services

        private readonly IMemoryService memory;

        #endregion
        #region fields

        private string _title = "Edlight";
        private WindowState _currentState;
        private LoaderModel _loader;

        #endregion
        #region props

        public string Title { get => _title; set => SetProperty(ref _title, value); }
        public WindowState CurrentState { get => _currentState; set => SetProperty(ref _currentState, value); }
        public LoaderModel Loader { get => _loader; set => SetProperty(ref _loader, value); }

        #endregion
        #region commands

        public DelegateCommand MinimizeCommand { get; private set; }
        public DelegateCommand CloseCommand { get; private set; }

        #endregion
        #region ctor

        public MainWindowViewModel(IMemoryService memory)
        {
            Loader = new();

            this.memory = memory;

            MinimizeCommand = new DelegateCommand(() => CurrentState = StaticCommands.ChangeWindowState(CurrentState));
            CloseCommand = new DelegateCommand(StaticCommands.Shutdown);
        }

        #endregion
    }
}
