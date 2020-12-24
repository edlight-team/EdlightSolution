using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace EdlightDataClient
{
    public static class EdlightProxy
    {
        public static async Task<TypeReturn> Execute<TypeReturn>(Func<EdlightClient, TypeReturn> func)
        {
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);

            EdlightClient client = null;
            try
            {
                client = new EdlightClient(binding, new EndpointAddress("net.tcp://localhost:555//EdlightClient"));
                return await Task.FromResult(func.Invoke(client));
            }
            catch
            {
                client.Abort();
                throw;
            }
            finally
            {
                client.Close();
            }
        }
    }
}
