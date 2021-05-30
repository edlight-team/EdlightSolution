using ApplicationModels.Models;
using ApplicationModels.Models.CapacityExtendedModels;
using ApplicationServices.WebApiService;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Styles.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace EdlightDesktopClient.ViewModels.Schedule.CapacityWindows
{
    public class CreateDisciplineWindowViewModel : BindableBase, IDialogAware
    {
        #region services

        private readonly IWebApiService api;

        #endregion
        #region fields

        private LoaderModel _loader;
        private ImportedDiscipline _imported;
        private AcademicDisciplinesModel _model;
        private ObservableCollection<AudiencesModel> _audiences;
        private AudiencesModel _selectedAudience;

        #endregion
        #region props

        public LoaderModel Loader { get => _loader; set => SetProperty(ref _loader, value); }
        public ImportedDiscipline Imported { get => _imported; set => SetProperty(ref _imported, value); }
        public AcademicDisciplinesModel Model { get => _model ??= new(); set => SetProperty(ref _model, value); }
        public ObservableCollection<AudiencesModel> Audiences { get => _audiences; set => SetProperty(ref _audiences, value); }
        public AudiencesModel SelectedAudience { get => _selectedAudience; set => SetProperty(ref _selectedAudience, value); }

        #endregion
        #region errors

        private bool _hasErrors;
        private string _errors;

        public bool HasErrors { get => _hasErrors; set => SetProperty(ref _hasErrors, value); }
        public string Errors { get => _errors ??= string.Empty; set => SetProperty(ref _errors, value); }

        #endregion
        #region commands

        private DelegateCommand _confirmCommand;
        private DelegateCommand<string> _closeDialogCommand;

        public DelegateCommand ConfirmCommand { get => _confirmCommand; set => SetProperty(ref _confirmCommand, value); }
        public DelegateCommand<string> CloseDialogCommand { get => _closeDialogCommand; set => SetProperty(ref _closeDialogCommand, value); }

        #endregion
        #region ctor

        public CreateDisciplineWindowViewModel(IWebApiService api)
        {
            Loader = new();
            this.api = api;

            CloseDialogCommand = new DelegateCommand<string>(CloseDialog);
            ConfirmCommand = new DelegateCommand(OnConfirm, CanConfirm)
                .ObservesProperty(() => Model.Title);
        }

        #endregion
        #region methods

        private bool CanConfirm()
        {
            HasErrors = false;
            Errors = string.Empty;
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
                Model.IdPriorityAudience = SelectedAudience == null ? Guid.Empty : SelectedAudience.Id;
                AcademicDisciplinesModel posted = await api.PostModel(Model, WebApiTableNames.AcademicDisciplines);
                Model = posted;
                CloseDialog(true.ToString());
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

        #endregion
        #region close dialog

        public string Title { get; } = "Создание дисциплины";
        public event Action<IDialogResult> RequestClose;
        protected virtual void CloseDialog(string parameter)
        {
            ButtonResult result = ButtonResult.None;

            if (parameter?.ToLower() == "true")
                result = ButtonResult.OK;
            else if (parameter?.ToLower() == "false")
                result = ButtonResult.Cancel;

            RaiseRequestClose(new DialogResult(result));
        }
        public virtual void RaiseRequestClose(IDialogResult dialogResult) => RequestClose?.Invoke(dialogResult);
        public bool CanCloseDialog() => true;

        #endregion
        #region dialog aware

        public void OnDialogClosed()
        {
            if (Model.Id != Guid.Empty)
            {
                Imported.LookUpDiscipline = Model;
                Imported.IsLookUpOnDB = true;
                Imported.IsConfirmedOrSkipped = true;
            }
        }
        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey(nameof(Audiences)) && parameters.GetValue<ObservableCollection<AudiencesModel>>(nameof(Audiences)) is ObservableCollection<AudiencesModel> navAud)
            {
                Audiences = navAud;
            }
            if (parameters.ContainsKey("ImportedDiscipline") && parameters.GetValue<ImportedDiscipline>("ImportedDiscipline") is ImportedDiscipline it)
            {
                Imported = it;
                Model.Title = Imported.DisciplineTitle;
            }
        }

        #endregion
    }
}
