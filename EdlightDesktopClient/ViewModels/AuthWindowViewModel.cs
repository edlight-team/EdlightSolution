using ApplicationExceptions.Exceptions;
using ApplicationModels.Models;
using ApplicationServices.HashingService;
using ApplicationServices.WebApiService;
using ApplicationWPFServices.MemoryService;
using ApplicationWPFServices.NotificationService;
using EdlightDesktopClient.BaseMethods;
using EdlightDesktopClient.Views;
using Prism.Commands;
using Prism.Mvvm;
using Styles.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unity;

namespace EdlightDesktopClient.ViewModels
{
    public sealed class AuthWindowViewModel : BindableBase
    {
        #region services

        private readonly IUnityContainer container;
        private readonly IHashingService hashing;
        private readonly IWebApiService api;
        private readonly INotificationService notification;
        private readonly IMemoryService memory;

        #endregion
        #region fields

        private string _title = "Edlight";
        private string _login;
        private string _password;
        private LoaderModel _loader;
        private Visibility _authVisibility;

        #endregion
        #region props

        public string Title { get => _title; set => SetProperty(ref _title, value); }
        public string Login { get => _login ??= string.Empty; set => SetProperty(ref _login, value); }
        public string Password { get => _password ??= string.Empty; set => SetProperty(ref _password, value); }
        public LoaderModel Loader { get => _loader; set => SetProperty(ref _loader, value); }
        public Visibility AuthVisibility { get => _authVisibility; set => SetProperty(ref _authVisibility, value); }

        #endregion
        #region commands

        public DelegateCommand CloseCommand { get; private set; }
        public DelegateCommand AuthCommand { get; private set; }

        #endregion
        #region ctor
        public AuthWindowViewModel(IUnityContainer container, IWebApiService api, IHashingService hashing, INotificationService notification, IMemoryService memory)
        {
            AuthVisibility = Visibility.Visible;
            Loader = new();

            this.container = container;
            this.api = api;
            this.hashing = hashing;
            this.notification = notification;
            this.memory = memory;

#if DEBUG
            Login = "admin";
            Password = "admin";
#endif

            CloseCommand = new DelegateCommand(StaticCommands.Shutdown);
            AuthCommand = new DelegateCommand(OnAuth, CanAuth)
                .ObservesProperty(() => Login)
                .ObservesProperty(() => Password);
        }
        #endregion
        #region methods
        private bool CanAuth()
        {
            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password)) return false;
            return true;
        }
        private async void OnAuth()
        {
            try
            {
                Loader = new("Выполняется загрузка");
#if DEBUG
                await Task.Delay(1000);
#endif
                var users = await api.GetModels<UserModel>(WebApiTableNames.Users);
                var target_user = users.FirstOrDefault(u => u.Login == Login);
                if (target_user == null)
                    throw new NotFoundException("Пользователь не найден в системе. \r\nПовторите попытку или обратитесь в тех.поддержку edlight@list.ru");
                if (target_user.Password != hashing.EncodeString(Password))
                    throw new AuthorizationException("Введенная пара логин/пароль не верные, проверьте ввод и повторите попытку.");

                memory.StoreItem(MemoryAlliases.CurrentUser, target_user);
                Loader = new();
                AuthVisibility = Visibility.Collapsed;
                var shell = container.Resolve<MainWindow>();
                shell.Show();
            }
            catch (NotFoundException nfe) { notification.ShowError(nfe.Message); }
            catch (AuthorizationException ae) { notification.ShowError(ae.Message); }
            catch (Exception)
            {
                notification.ShowError($"При попытке подключения к серверу произошла неизвестная ошибка. \r\nОбратитесь в тех.поддержку edlight@list.ru");
            }
            finally
            {
                Loader = new();
            }
        }
        #endregion
    }
}
