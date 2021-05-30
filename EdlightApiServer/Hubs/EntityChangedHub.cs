using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace EdlightApiServer.Hubs
{
    public class EntityChangedHub : Hub
    {
        public async Task SendEntity(string serialized_entity)
        {
            await Clients.Others.SendAsync("EntityChanged", serialized_entity);
        }
    }
}
