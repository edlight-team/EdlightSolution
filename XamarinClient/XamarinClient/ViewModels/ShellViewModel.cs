using System;
using System.Net;
using System.Net.Http;
using System.Windows.Input;
using ApplicationModels.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Xamarin.Forms;
using XamarinClient.Views.MainViews;

namespace XamarinClient.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private UserModel model;
        public UserModel Model
        {
            get => model ??= new();
            set => SetProperty(ref model, value);
        }

        public ICommand AuthCommand { get; }

        public ShellViewModel()
        {
#if DEBUG
            Model.Login = "admin";
            Model.Password = "admin";
#endif

            AuthCommand = new DelegateCommand(OnAuthCommand);
        }

        private async void OnAuthCommand()
        {
            try
            {
                HttpClient client = new();
                client.Timeout = TimeSpan.FromSeconds(5);
                HttpRequestMessage request = new();
                request.RequestUri = new Uri($"http://188.43.234.133:5000/api/users/login={Model.Login}&auth_token=5B6253853ACCF8B8E4FEE1F67C46D");
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
                    if (user.Password != Model.Password)
                    {
                        await Application.Current.MainPage.DisplayAlert("Ошибка", "Пароль не подходит", "Ок");
                        return;
                    }
                    await Application.Current.MainPage.Navigation.PushModalAsync(new OnLoginExecutedView());
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", ex.Message, "Ок");
            }
        }
    }
}
