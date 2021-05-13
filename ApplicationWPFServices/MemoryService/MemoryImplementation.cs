using System;
using System.Runtime.Caching;

namespace ApplicationWPFServices.MemoryService
{
    public class MemoryImplementation : IMemoryService
    {
        private static MemoryCache cache;
        public MemoryImplementation() => cache = MemoryCache.Default;
        public void StoreItem<TData>(string alias, TData item)
        {
            CacheItemPolicy policy = new();
            policy.SlidingExpiration = TimeSpan.FromMinutes(0);
            cache.Set(alias, item, policy);
        }
        public TData GetItem<TData>(string alias)
        {
            var data = cache.Get(alias);
            if (data is TData item)
            {
                return item;
            }
            return default;
        }
    }
}
