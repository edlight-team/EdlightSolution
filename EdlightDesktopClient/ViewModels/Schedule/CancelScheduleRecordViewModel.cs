using ApplicationEventsWPF.Events.ScheduleEvents;
using ApplicationModels.Models;
using ApplicationServices.WebApiService;
using HandyControl.Controls;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Styles.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EdlightDesktopClient.ViewModels.Schedule
{
    public class CancelScheduleRecordViewModel : BindableBase, INavigationAware
    {
        #region services
        private readonly IRegionManager manager;
        private readonly IWebApiService api;
        private readonly IEventAggregator aggregator;
        #endregion
        #region fields
        private LoaderModel _loader;
        private string _reason;
        private Guid _recordId;
        #endregion
        #region props
        public LoaderModel Loader { get => _loader; set => SetProperty(ref _loader, value); }
        public string Reason { get => _reason; set => SetProperty(ref _reason, value); }
        public Guid RecordId { get => _recordId; set => SetProperty(ref _recordId, value); }
        #endregion
        #region errors

        private bool _hasErrors;
        private string _timeToSmallError;

        public bool HasErrors { get => _hasErrors; set => SetProperty(ref _hasErrors, value); }
        public string Errors { get => _timeToSmallError ??= string.Empty; set => SetProperty(ref _timeToSmallError, value); }

        #endregion
        #region commands
        public DelegateCommand ConfirmCancelRecordCommand { get; private set; }
        public DelegateCommand CloseModalCommand { get; private set; }
        #endregion
        #region ctor
        public CancelScheduleRecordViewModel(IRegionManager manager, IWebApiService api, IEventAggregator aggregator)
        {
            Loader = new();
            this.manager = manager;
            this.api = api;
            this.aggregator = aggregator;

            CloseModalCommand = new DelegateCommand(OnCloseModal);
            ConfirmCancelRecordCommand = new DelegateCommand(ConfirmCancelRecord, CanConfirmCancelRecord).ObservesProperty(() => Reason);
        }
        #endregion
        #region methods
        private void OnCloseModal() => manager.Regions[BaseMethods.RegionNames.ModalRegion].RemoveAll();
        private bool CanConfirmCancelRecord()
        {
            HasErrors = false;
            if (string.IsNullOrEmpty(Reason))
            {
                Errors = "Причина отмены занятия обязательна к заполнению";
                HasErrors = true;
            }
            return !HasErrors;
        }
        private async void ConfirmCancelRecord()
        {
            try
            {
                Loader.SetDefaultLoadingInfo();
                List<LessonsModel> api_records = await api.GetModels<LessonsModel>(WebApiTableNames.Lessons, $"Id = '{RecordId}'");
                LessonsModel target = api_records.FirstOrDefault();
                target.CanceledReason = Reason;
                await api.PutModel(target, WebApiTableNames.Lessons);

                aggregator.GetEvent<DateChangedEvent>().Publish();
                Growl.Info("Запись успешно обновлена", "Global");
                OnCloseModal();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await Loader.Clear();
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("RecordId") && navigationContext.Parameters["RecordId"] is Guid record)
            {
                RecordId = record;
            }
        }
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;
        public void OnNavigatedFrom(NavigationContext navigationContext) { }
        #endregion
    }
}
