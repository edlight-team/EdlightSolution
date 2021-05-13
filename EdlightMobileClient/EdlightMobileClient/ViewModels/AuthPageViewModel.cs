using ApplicationModels.Models;
using ApplicationServices.HashingService;
using ApplicationServices.WebApiService;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using StaticCollections;
using System;
using System.Net;
using System.Net.Http;
using Xamarin.Forms;

namespace EdlightMobileClient.ViewModels
{
    public class AuthPageViewModel : ViewModelBase
    {
        public AuthPageViewModel(INavigationService navigationService, IHashingService hashing) : base(navigationService)
        {
#if DEBUG
            Model.Login = "admin";
            Model.Password = "admin";
#endif
            this.hashing = hashing;
            //this.api = api;
            AuthCommand = new DelegateCommand(OnAuthCommand);
        }

        private readonly IHashingService hashing;
        private readonly IWebApiService api;

        private UserModel model;
        public UserModel Model { get => model ??= new(); set => SetProperty(ref model, value); }

        public DelegateCommand AuthCommand { get; }
        private async void OnAuthCommand()
        {
            try
            {
                //ToDo: Переделай вместо этого на api.GetModels..........
                //Пример в EdlightSolution -> ClientDesktop -> EdlightDesktopClient -> ViewModels -> AuthWindowViewModel

                HttpClient client = new();
                client.Timeout = TimeSpan.FromSeconds(5);
                HttpRequestMessage request = new();
                request.RequestUri = new Uri($"{StaticStrings.BaseURL}/api/users/login={Model.Login}&auth_token=5B6253853ACCF8B8E4FEE1F67C46D");
                request.Method = HttpMethod.Get;
                request.Headers.Add("Accept", "application/json");

                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", response.Content.ReadAsStringAsync().Result, "Ок");
                    return;
                }
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", response.Content.ReadAsStringAsync().Result, "Ок");
                    return;
                }
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    HttpContent responseContent = response.Content;
                    string json = await responseContent.ReadAsStringAsync();
                    UserModel user = JsonConvert.DeserializeObject<UserModel>(json);
                    string hashPass = hashing.GetHash(Model.Password);
                    if (user.Password != hashPass)
                    {
                        await Application.Current.MainPage.DisplayAlert("Ошибка", "Пароль не подходит", "Ок");
                        return;
                    }
                    await NavigationService.NavigateAsync("NavigationPage/ShellTabbedPage?selectedTab=WeekSchedulePage");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", ex.Message, "Ок");
            }
        }
    }
}
