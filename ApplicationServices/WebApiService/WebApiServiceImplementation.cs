using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ApplicationServices.WebApiService
{
    public class WebApiServiceImplementation : IWebApiService
    {
        private static readonly string WebApiBaseURL = "http://62.173.147.98:500";
        private WebRequest CreateRequest(string api, string method)
        {
            WebRequest request = WebRequest.CreateHttp(WebApiBaseURL + api);
            request.Timeout = 60000;
            request.Method = method;
            return request;
        }
        public async Task<IEnumerable<TData>> GetModels<TData>(string apiName)
        {
            WebRequest request = CreateRequest(apiName, WebRequestMethods.Http.Get);
            WebResponse response = await request.GetResponseAsync();
            using Stream response_stream = response.GetResponseStream();
            using StreamReader reader = new(response_stream);
            string result = string.Empty;
            result = await reader.ReadToEndAsync();
            reader.Close();

            return JsonConvert.DeserializeObject<IEnumerable<TData>>(result);
        }
        public async Task<TData> PostModel<TData>(TData item, string apiName)
        {
            WebRequest request = CreateRequest(apiName, WebRequestMethods.Http.Get);



            WebResponse response = await request.GetResponseAsync();
            using Stream response_stream = response.GetResponseStream();
            using StreamReader reader = new(response_stream);
            string result = string.Empty;
            result = await reader.ReadToEndAsync();
            reader.Close();

            return JsonConvert.DeserializeObject<TData>(result);
        }
        public Task<TData> PutModel<TData>(TData item, string apiName)
        {
            throw new NotImplementedException();
        }
        public Task<int> DeleteModel<TData>(Guid id, string apiName)
        {
            throw new NotImplementedException();
        }
    }
}
