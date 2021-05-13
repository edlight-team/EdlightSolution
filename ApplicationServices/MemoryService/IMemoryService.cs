namespace ApplicationServices.MemoryService
{
    public interface IMemoryService
    {
        void StoreItem<TData>(string alias, TData item);
        TData GetItem<TData>(string alias);
    }
}
