using System;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Windows.Input;
using ApplicationModels;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using TestApp.Views;
using Xamarin.Forms;

namespace TestApp.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        private UserModel model;
        public UserModel Model
        {
            get => model ??= new();
            set => SetProperty(ref model, value);
        }

        public ICommand AuthCommand { get; }
        public MainPageViewModel()
        {
#if DEBUG
            Model.Login = "admin";
            Model.Password = "admin";
#endif

            AuthCommand = new DelegateCommand(OnSomeCommand);
        }

        private async void OnSomeCommand()
        {
            try
            {
                HttpClient client = new();
                client.Timeout = TimeSpan.FromSeconds(5);
                HttpRequestMessage request = new();
                request.RequestUri = new Uri($"http://192.168.0.11:5000/api/users/login={Model.Login}&auth_token=5B6253853ACCF8B8E4FEE1F67C46D");
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
                    var json = await responseContent.ReadAsStringAsync();
                    UserModel user = JsonConvert.DeserializeObject<UserModel>(json);
                    if (user.Password != Model.Password)
                    {
                        await Application.Current.MainPage.DisplayAlert("Ошибка", "Пароль не подходит", "Ок");
                        return;
                    }
                    await Application.Current.MainPage.Navigation.PushModalAsync(new TabbedPage1());
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", ex.Message, "Ок");
            }
        }
    }
}
