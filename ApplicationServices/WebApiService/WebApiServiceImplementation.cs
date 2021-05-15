using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices.WebApiService
{
    public class WebApiServiceImplementation : IWebApiService
    {
#if DEBUG
        //private static readonly string WebApiBaseURL = "http://192.168.0.100:1000/api/";
        private static readonly string WebApiBaseURL = "http://62.173.154.96:600/api/";
#else
        private static readonly string WebApiBaseURL = "http://62.173.154.96:600/api/";
#endif
        private WebRequest CreateRequest(string api, string method)
        {
            WebRequest request = WebRequest.CreateHttp(WebApiBaseURL + api);
            request.Timeout = 10000;
            request.Method = method;
            return request;
        }
        public async Task<List<TData>> GetModels<TData>(string apiName, string condition)
        {
            WebRequest request = CreateRequest(apiName, WebRequestMethods.Http.Get);
            request.Headers.Add("condition", condition);
            WebResponse response = await request.GetResponseAsync();
            using Stream response_stream = response.GetResponseStream();
            using StreamReader reader = new(response_stream);
            string result = string.Empty;
            result = await reader.ReadToEndAsync();
            reader.Close();

            return JsonConvert.DeserializeObject<List<TData>>(result);
        }
        public async Task<List<TData>> GetModels<TData>(string apiName)
        {
            WebRequest request = CreateRequest(apiName, WebRequestMethods.Http.Get);
            WebResponse response = await request.GetResponseAsync();
            using Stream response_stream = response.GetResponseStream();
            using StreamReader reader = new(response_stream);
            string result = string.Empty;
            result = await reader.ReadToEndAsync();
            reader.Close();

            return JsonConvert.DeserializeObject<List<TData>>(result);
        }
        public async Task<TData> PostModel<TData>(TData item, string apiName)
        {
            WebRequest request = CreateRequest(apiName, WebRequestMethods.Http.Post);

            string data = JsonConvert.SerializeObject(item);
            byte[] data_bytes = Encoding.UTF8.GetBytes(data);
            request.ContentType = "application/json";
            request.ContentLength = data_bytes.Length;

            using Stream requestStream = await request.GetRequestStreamAsync();
            await requestStream.WriteAsync(data_bytes, 0, data_bytes.Length);
            requestStream.Close();

            WebResponse response = await request.GetResponseAsync();
            using Stream response_stream = response.GetResponseStream();
            using StreamReader reader = new(response_stream);
            string result = string.Empty;
            result = await reader.ReadToEndAsync();
            reader.Close();

            return JsonConvert.DeserializeObject<TData>(result);
        }
        public async Task<TData> PutModel<TData>(TData item, string apiName)
        {
            WebRequest request = CreateRequest(apiName, WebRequestMethods.Http.Put);

            string data = JsonConvert.SerializeObject(item);
            byte[] data_bytes = Encoding.UTF8.GetBytes(data);
            request.ContentType = "application/json";
            request.ContentLength = data_bytes.Length;

            using Stream requestStream = await request.GetRequestStreamAsync();
            await requestStream.WriteAsync(data_bytes, 0, data_bytes.Length);
            requestStream.Close();

            WebResponse response = await request.GetResponseAsync();
            using Stream response_stream = response.GetResponseStream();
            using StreamReader reader = new(response_stream);
            string result = string.Empty;
            result = await reader.ReadToEndAsync();
            reader.Close();

            return JsonConvert.DeserializeObject<TData>(result);
        }
        public async Task<int> DeleteModel(Guid id, string apiName)
        {
            WebRequest request = CreateRequest(apiName, "DELETE");
            request.Headers.Add("id", id.ToString());
            WebResponse response = await request.GetResponseAsync();
            using Stream response_stream = response.GetResponseStream();
            using StreamReader reader = new(response_stream);
            string result = string.Empty;
            result = await reader.ReadToEndAsync();
            reader.Close();

            return int.Parse(result);
        }
        public async Task<string> DeleteAll(string Target = null)
        {
            WebRequest request = CreateRequest("Database", WebRequestMethods.Http.Get);
            if (Target != null) request.Headers.Add(nameof(Target), Target);
            WebResponse response = await request.GetResponseAsync();
            using Stream response_stream = response.GetResponseStream();
            using StreamReader reader = new(response_stream);
            string result = string.Empty;
            result = await reader.ReadToEndAsync();
            reader.Close();
            return result;
        }
    }
}
