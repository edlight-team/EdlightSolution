﻿using ApplicationEventsWPF.Events;
using ApplicationModels.Models;
using ApplicationServices.WebApiService;
using HandyControl.Controls;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Styles.Models;
using System;

namespace EdlightDesktopClient.ViewModels.Dictionaries
{
    [RegionMemberLifetime(KeepAlive = false)]
    public class EditDisciplinesViewModel : BindableBase, INavigationAware
    {
        #region services

        private readonly IRegionManager manager;
        private readonly IWebApiService api;
        private readonly IEventAggregator aggregator;

        #endregion
        #region fields

        private LoaderModel _loader;
        private AcademicDisciplinesModel _model;
        private string _saveButtonText;

        #endregion
        #region props

        public LoaderModel Loader { get => _loader; set => SetProperty(ref _loader, value); }
        public AcademicDisciplinesModel Model { get => _model ??= new(); set => SetProperty(ref _model, value); }
        public string SaveButtonText { get => _saveButtonText; set => SetProperty(ref _saveButtonText, value); }

        #endregion
        #region errors

        private bool _hasErrors;
        private string _errors;

        public bool HasErrors { get => _hasErrors; set => SetProperty(ref _hasErrors, value); }
        public string Errors { get => _errors ??= string.Empty; set => SetProperty(ref _errors, value); }

        #endregion
        #region commands

        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand ConfirmCommand { get; private set; }
        public DelegateCommand CloseModalCommand { get; private set; }

        #endregion
        #region ctor

        public EditDisciplinesViewModel(IRegionManager manager, IWebApiService api, IEventAggregator aggregator)
        {
            Loader = new();
            this.manager = manager;
            this.api = api;
            this.aggregator = aggregator;

            LoadedCommand = new DelegateCommand(OnLoaded);
            ConfirmCommand = new DelegateCommand(OnConfirm, CanConfirm).ObservesProperty(() => Model.Title);
            CloseModalCommand = new DelegateCommand(OnCloseModal);
        }
        private void OnLoaded() => SaveButtonText = string.IsNullOrEmpty(Model.Title) ? "Создать запись" : "Сохранить запись";

        #endregion
        #region methods

        private bool CanConfirm()
        {
            HasErrors = false;
            if (string.IsNullOrEmpty(Model.Title))
            {
                Errors += "Название дисциплины является обязательным";
                HasErrors = true;
            }
            return !HasErrors;
        }
        private async void OnConfirm()
        {
            try
            {
                Loader.SetDefaultLoadingInfo();
                if (SaveButtonText == "Создать запись")
                {
                    await api.PostModel(Model, WebApiTableNames.AcademicDisciplines);
                    aggregator.GetEvent<DictionaryModelChangedEvent>().Publish(Model);
                    Growl.Info("Запись успешно создана", "Global");
                    OnCloseModal();
                }
                else
                {
                    await api.PutModel(Model, WebApiTableNames.AcademicDisciplines);
                    Growl.Info("Запись успешно сохранена", "Global");
                    OnCloseModal();
                }
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
            if (navigationContext.Parameters.ContainsKey(nameof(Model)) && navigationContext.Parameters[nameof(Model)] is AcademicDisciplinesModel navModel)
            {
                Model = navModel;
            }
        }
        public void OnNavigatedFrom(NavigationContext navigationContext) { }
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        #endregion
    }
}
