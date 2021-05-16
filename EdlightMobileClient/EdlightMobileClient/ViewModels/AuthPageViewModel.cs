using ApplicationModels.Models;
using ApplicationServices.HashingService;
using ApplicationServices.WebApiService;
using ApplicationXamarinService.MemoryService;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using StaticCollections;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EdlightMobileClient.ViewModels
{
    public class AuthPageViewModel : ViewModelBase
    {
        #region services
        private readonly IHashingService hashing;
        private readonly IWebApiService api;
        private readonly IMemoryService memory;
        #endregion
        #region fields
        private UserModel model;

        #endregion
        #region props
        public UserModel Model { get => model ??= new(); set => SetProperty(ref model, value); }

        #endregion
        #region commands
        public DelegateCommand AuthCommand { get; }

        #endregion
        #region constructor
        public AuthPageViewModel(INavigationService navigationService, IWebApiService api, IMemoryService memory, IHashingService hashing) : base(navigationService)
        {
#if DEBUG
            //Model.Login = "admin";
            //Model.Password = "admin";
            //Model.Login = "student";
            //Model.Password = "student";
            Model.Login = "teacher";
            Model.Password = "teacher";
#endif
            this.hashing = hashing;
            this.api = api;
            this.memory = memory;

            AuthCommand = new DelegateCommand(OnAuthCommand);
        }
        #endregion
        #region methods
        private async void OnAuthCommand()
        {
            try
            {
                var users = await api.GetModels<UserModel>(WebApiTableNames.Users);
                var target_user = users.FirstOrDefault(u => u.Login == model.Login);

                if (target_user.Password != hashing.EncodeString(model.Password))
                    throw new Exception();

                memory.StoreItem(MemoryAlliases.CurrentUser, target_user);

                await NavigationService.NavigateAsync("ShellTabbedPage?selectedTab=WeekSchedulePage");
            }
            catch (Exception)
            {
            }
        }

        #endregion
    }
}
