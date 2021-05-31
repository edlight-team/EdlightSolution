using ApplicationModels.Models;
using ApplicationModels.Models.CapacityExtendedModels;
using ApplicationServices.HashingService;
using ApplicationServices.TranslationService;
using ApplicationServices.WebApiService;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Styles.Models;
using System;
using System.Linq;

namespace EdlightDesktopClient.ViewModels.Schedule.CapacityWindows
{
    public class CreateTeacherWindowViewModel : BindableBase, IDialogAware
    {
        #region services

        private readonly ITranslationService translation;
        private readonly IHashingService hashing;
        private readonly IWebApiService api;

        #endregion
        #region fields

        private LoaderModel _loader;
        private ImportedTeacher _imported;
        private UserModel _model;
        private int[] _prioritetes;

        #endregion
        #region props

        public LoaderModel Loader { get => _loader; set => SetProperty(ref _loader, value); }
        public ImportedTeacher Imported { get => _imported; set => SetProperty(ref _imported, value); }
        public UserModel Model { get => _model ??= new(); set => SetProperty(ref _model, value); }
        public int[] Prioritetes { get => _prioritetes; set => SetProperty(ref _prioritetes, value); }

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

        public CreateTeacherWindowViewModel(ITranslationService translation, IHashingService hashing, IWebApiService api)
        {
            Loader = new();
            Prioritetes = new int[6] { 0, 0, 0, 0, 0, 0 };
            this.translation = translation;
            this.hashing = hashing;
            this.api = api;

            CloseDialogCommand = new DelegateCommand<string>(CloseDialog);
            ConfirmCommand = new DelegateCommand(OnConfirm, CanConfirm)
                .ObservesProperty(() => Model.Login)
                .ObservesProperty(() => Model.Password)
                .ObservesProperty(() => Model.Name)
                .ObservesProperty(() => Model.Surname)
                .ObservesProperty(() => Model.Patrnymic);
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
            if (string.IsNullOrEmpty(Model.Login) || string.IsNullOrEmpty(Model.Password))
            {
                Errors += Environment.NewLine + "Пара логин/пароль обязательны к заполнению";
                HasErrors = true;
            }
            return !HasErrors;
        }
        private async void OnConfirm()
        {
            string old_pass = Model.Password;
            try
            {
                Loader.SetDefaultLoadingInfo();

                Model.DaysPriority = new System.Collections.Generic.List<int>(Prioritetes);
                Model.Password = hashing.EncodeString(Model.Password);

                RolesModel teach_role = (await api.GetModels<RolesModel>(WebApiTableNames.Roles, "RoleName = 'teacher'")).FirstOrDefault();
                UserModel postedUser = await api.PostModel(Model, WebApiTableNames.Users);
                await api.PostModel(new UsersRolesModel() { IdRole = teach_role.Id, IdUser = postedUser.ID }, WebApiTableNames.UsersRoles);
                Model = postedUser;

                CloseDialog(true.ToString());
            }
            catch (Exception)
            {
                Model.Password = old_pass;
                throw;
            }
            finally
            {
                await Loader.Clear();
            }
        }

        #endregion
        #region close dialog

        public string Title { get; } = "Создание преподавателя";
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
            if (Model.ID != Guid.Empty)
            {
                Imported.LookUpUser = Model;
                Imported.IsLookUpOnDB = true;
                Imported.IsConfirmedOrSkipped = true;
            }
        }
        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("ImportedTeacher") && parameters.GetValue<ImportedTeacher>("ImportedTeacher") is ImportedTeacher it)
            {
                Imported = it;
                string surname = it.TeacherInitials.Split().FirstOrDefault();
                string[] initials = it.TeacherInitials.Split().LastOrDefault().Split('.');
                Model.Name = initials[0];
                Model.Patrnymic = initials[1];
                Model.Surname = surname;
                Model.Login = translation.TranslateWord(surname + Model.Name + Model.Patrnymic);
                Model.Password = Model.Login;
            }
        }

        #endregion
    }
}
