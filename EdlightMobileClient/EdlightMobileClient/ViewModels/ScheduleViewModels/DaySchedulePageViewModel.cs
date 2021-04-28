using EdlightMobileClient.Collections;
using EdlightMobileClient.Models;
using Prism.Commands;
using Prism.Navigation;

namespace EdlightMobileClient.ViewModels.ScheduleViewModels
{
    public class DaySchedulePageViewModel : ViewModelBase
    {
        #region field
        private IndexableObservableCollection<DayModel> selectedWeek;
        #endregion

        #region prop
        public IndexableObservableCollection<DayModel> SelectedWeek
        {
            set { SetProperty(ref selectedWeek, value); }
            get { return selectedWeek; }
        }

        public int StartChildIndex { get; private set; }
        #endregion

        #region command
        public DelegateCommand NavigateBackCommand;
        private async void ExecuteNavigateBackCommand()
        {
            await NavigationService.GoBackAsync();
        }
        #endregion

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            StartChildIndex = parameters.GetValue<int>("index");
            SelectedWeek = parameters.GetValue<IndexableObservableCollection<DayModel>>("model");
        }

        public DaySchedulePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            NavigateBackCommand = new DelegateCommand(ExecuteNavigateBackCommand);

        }
    }
}
