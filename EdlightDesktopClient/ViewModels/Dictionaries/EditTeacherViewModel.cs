using ApplicationEventsWPF.Events;
using ApplicationModels.Models;
using ApplicationServices.HashingService;
using ApplicationServices.WebApiService;
using HandyControl.Controls;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Styles.Models;
using System;
using System.Linq;

namespace EdlightDesktopClient.ViewModels.Dictionaries
{
    [RegionMemberLifetime(KeepAlive = false)]
    public class EditTeacherViewModel : BindableBase, INavigationAware
    {
        #region services

        private readonly IRegionManager manager;
        private readonly IWebApiService api;
        private readonly IEventAggregator aggregator;
        private readonly IHashingService hashing;

        #endregion
        #region fields

        private LoaderModel _loader;
        private UserModel _model;
        private string _saveButtonText;
        private int[] _prioritetes;

        #endregion
        #region props

        public LoaderModel Loader { get => _loader; set => SetProperty(ref _loader, value); }
        public UserModel Model { get => _model ??= new(); set => SetProperty(ref _model, value); }
        public string SaveButtonText { get => _saveButtonText; set => SetProperty(ref _saveButtonText, value); }
        public int[] Prioritetes { get => _prioritetes; set => SetProperty(ref _prioritetes, value); }

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

        public EditTeacherViewModel(IRegionManager manager, IWebApiService api, IEventAggregator aggregator, IHashingService hashing)
        {
            Loader = new();
            this.manager = manager;
            this.api = api;
            this.aggregator = aggregator;
            this.hashing = hashing;

            LoadedCommand = new DelegateCommand(OnLoaded);
            ConfirmCommand = new DelegateCommand(OnConfirm, CanConfirm)
                .ObservesProperty(() => Model.Login)
                .ObservesProperty(() => Model.Password)
                .ObservesProperty(() => Model.Name)
                .ObservesProperty(() => Model.Surname)
                .ObservesProperty(() => Model.Patrnymic);
            CloseModalCommand = new DelegateCommand(OnCloseModal);
        }
        private void OnLoaded()
        {
            SaveButtonText = string.IsNullOrEmpty(Model.Name) ? "Создать запись" : "Сохранить запись";
            if (SaveButtonText == "Создать запись")
            {
                Prioritetes = new int[6] { 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                Prioritetes = Model.DaysPriority.ToArray();
            }
            CanConfirm();
        }

        #endregion
        #region methods

        private bool CanConfirm()
        {
            HasErrors = false;
            Errors = string.Empty;
            if (string.IsNullOrEmpty(Model.Name) || string.IsNullOrEmpty(Model.Surname) || string.IsNullOrEmpty(Model.Patrnymic))
            {
                Errors += "ФИО преподавателя является обязательным";
                HasErrors = true;
            }
            if (SaveButtonText == "Создать запись" && (string.IsNullOrEmpty(Model.Login) || string.IsNullOrEmpty(Model.Password)))
            {
                Errors += Environment.NewLine + "Пара логин/пароль обязательны к заполнению";
                HasErrors = true;
            }
            return !HasErrors;
        }
        private async void OnConfirm()
        {
            try
            {
                Loader.SetDefaultLoadingInfo();

                Model.DaysPriority = new System.Collections.Generic.List<int>(Prioritetes);

                if (SaveButtonText == "Создать запись")
                {
                    Model.Password = hashing.EncodeString(Model.Password);

                    RolesModel teach_role = (await api.GetModels<RolesModel>(WebApiTableNames.Roles, "RoleName = 'teacher'")).FirstOrDefault();
                    UserModel postedUser = await api.PostModel(Model, WebApiTableNames.Users);
                    await api.PostModel(new UsersRolesModel() { IdRole = teach_role.Id, IdUser = postedUser.ID }, WebApiTableNames.UsersRoles);

                    aggregator.GetEvent<DictionaryModelChangedEvent>().Publish(Model);
                    Growl.Info("Запись успешно создана", "Global");
                    OnCloseModal();
                }
                else
                {
                    await api.PutModel(Model, WebApiTableNames.Users);
                    aggregator.GetEvent<DictionaryModelChangedEvent>().Publish(Model);
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
            if (navigationContext.Parameters.ContainsKey(nameof(Model)) && navigationContext.Parameters[nameof(Model)] is UserModel navModel)
            {
                Model = navModel;
            }
        }
        public void OnNavigatedFrom(NavigationContext navigationContext) { }
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        #endregion
    }
}
