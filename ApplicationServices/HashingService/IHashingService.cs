namespace ApplicationServices.HashingService
{
    public interface IHashingService
    {
        string GetHash(string text);
        string EncodeString(string source);
        string DecodeString(string source);
    }
}
