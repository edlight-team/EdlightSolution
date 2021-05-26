using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace ApplicationServices.SignalClientSerivce
{
    public class SignalClientImplementation : ISignalClientService
    {
        private static readonly string WebApiBaseURL = "http://62.173.154.96:600/";
        //private static readonly string WebApiBaseURL = "http://192.168.0.100:600/";
        private static readonly string EntityChangedHubName = "EntityChangedHub";
        private static readonly string EntityChangedMethodName = "EntityChanged";
        private static readonly string SendEntityMethodName = "SendEntity";
        private static HubConnection EntityChangedHub;

        public async Task SubscribeEntityChanged(Action<string> handler)
        {
            try
            {
                EntityChangedHub = new HubConnectionBuilder()
                    .WithUrl($"{WebApiBaseURL}{EntityChangedHubName}")
                    .Build();
                EntityChangedHub.On(EntityChangedMethodName, handler);
                await EntityChangedHub.StartAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task UnsubscribeEntityChanged() => await EntityChangedHub.DisposeAsync();
        public async Task SendEntityModel(string serializedEntity)
        {
            if (EntityChangedHub == null) return;
            await EntityChangedHub?.InvokeAsync(SendEntityMethodName, serializedEntity);
        }
    }
}
