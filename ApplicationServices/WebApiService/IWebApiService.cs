using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationServices.WebApiService
{
    public interface IWebApiService
    {
        Task<IEnumerable<TData>> GetModels<TData>(string apiName);
        Task<TData> PostModel<TData>(TData item, string apiName);
        Task<TData> PutModel<TData>(TData item, string apiName);
        Task<int> DeleteModel(Guid id, string apiName);
    }
}
