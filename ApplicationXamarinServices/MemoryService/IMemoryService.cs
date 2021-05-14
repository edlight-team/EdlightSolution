using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationXamarinServices.MemoryService
{
    public interface IMemoryService
    {
        void StoreItem<TData>(string alias, TData item);
        TData GetItem<TData>(string alias);
    }
}
