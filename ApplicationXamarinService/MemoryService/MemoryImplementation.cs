﻿using Newtonsoft.Json;
using Xamarin.Essentials;

namespace ApplicationXamarinService.MemoryService
{
    public class MemoryImplementation : IMemoryService
    {
        public void StoreItem<TData>(string alias, TData item)
        {
            string serializeModel = JsonConvert.SerializeObject(item);
            Preferences.Set(alias, serializeModel);
        }
        public TData GetItem<TData>(string alias)
        {
            try
            {
                string serializedObject = Preferences.Get(alias, string.Empty);
                if (!string.IsNullOrEmpty(serializedObject))
                {
                    TData data = JsonConvert.DeserializeObject<TData>(serializedObject);
                    return data;
                }
                return default;
            }
            catch
            {
                return default;
            }
        }
    }
}
