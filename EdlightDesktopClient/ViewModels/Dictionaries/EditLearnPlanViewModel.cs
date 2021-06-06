using ApplicationEventsWPF.Events;
using ApplicationModels.Models;
using ApplicationServices.WebApiService;
using HandyControl.Controls;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Styles.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace EdlightDesktopClient.ViewModels.Dictionaries
{
    [RegionMemberLifetime(KeepAlive = false)]
    public class EditLearnPlanViewModel : BindableBase, INavigationAware
    {
        #region services

        private readonly IRegionManager manager;
        private readonly IWebApiService api;
        private readonly IEventAggregator aggregator;

        #endregion
        #region fields

        private LoaderModel _loader;
        private LearnPlanesModel _model;
        private string _planPath;

        #endregion
        #region props

        public LoaderModel Loader { get => _loader; set => SetProperty(ref _loader, value); }
        public LearnPlanesModel Model { get => _model ??= new(); set => SetProperty(ref _model, value); }
        public string PlanPath { get => _planPath; set => SetProperty(ref _planPath, value); }

        #endregion
        #region errors

        private bool _hasErrors;
        private string _errors;

        public bool HasErrors { get => _hasErrors; set => SetProperty(ref _hasErrors, value); }
        public string Errors { get => _errors ??= string.Empty; set => SetProperty(ref _errors, value); }

        #endregion
        #region commands

        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand OpenFileDialogCommand { get; private set; }
        public DelegateCommand ConfirmCommand { get; private set; }
        public DelegateCommand CloseModalCommand { get; private set; }

        #endregion
        #region ctor

        public EditLearnPlanViewModel(IRegionManager manager, IWebApiService api, IEventAggregator aggregator)
        {
            Loader = new();
            this.manager = manager;
            this.api = api;
            this.aggregator = aggregator;

            LoadedCommand = new DelegateCommand(OnLoaded);
            OpenFileDialogCommand = new DelegateCommand(OnSetFilePath);
            ConfirmCommand = new DelegateCommand(OnConfirm, CanConfirm)
                .ObservesProperty(() => Model.Name)
                .ObservesProperty(() => PlanPath);
            CloseModalCommand = new DelegateCommand(OnCloseModal);
        }
        private void OnLoaded()
        {

        }

        #endregion
        #region methods

        private void OnSetFilePath()
        {
            OpenFileDialog ofd = new();
            ofd.Filter = "Edlight learn plan table (*.xlsx)|*.xlsx";

            bool? result = ofd.ShowDialog();
            if(!result.Value) return;

            Model.Name = ofd.SafeFileName.Remove(ofd.SafeFileName.LastIndexOf('.'));
            PlanPath = ofd.FileName;
        }
        private bool CanConfirm()
        {
            Errors = string.Empty;
            HasErrors = false;
            if (string.IsNullOrEmpty(Model.Name))
            {
                Errors += "Название учебного плана является обязательным";
                HasErrors = true;
            }
            if (string.IsNullOrEmpty(PlanPath))
            {
                Errors += "Путь к учебному плану является обязательным";
                HasErrors = true;
            }
            return !HasErrors;
        }
        private async void OnConfirm()
        {
            try
            {
                Loader.SetDefaultLoadingInfo();

                byte[] data = System.IO.File.ReadAllBytes(PlanPath);

                string path = await api.PushLearnPlan("/", new ApplicationModels.JsonFileModel() { FileName = Model.Name + ".xlsx", Data = data });

                Model.Path = Model.Name + ".xlsx";
                LearnPlanesModel posted = await api.PostModel<LearnPlanesModel>(Model, WebApiTableNames.LearnPlanes);

                aggregator.GetEvent<DictionaryModelChangedEvent>().Publish(posted);
                Growl.Info("Запись успешно создана", "Global");
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
        private void OnCloseModal() => manager.Regions[BaseMethods.RegionNames.ModalRegion].RemoveAll();

        #endregion
        #region navigation

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }
        public void OnNavigatedFrom(NavigationContext navigationContext) { }
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        #endregion
    }
}
