using BotCrypt;
using System.Security.Cryptography;
using System.Text;

namespace ApplicationServices.HashingService
{
    public class HashingImplementation : IHashingService
    {
        private readonly string password = "EdLiGhTCrYpToPaSsWoRd!!____DSTU";
        public string GetHash(string text)
        {
            SHA256Managed sha256Managed = new();
            byte[] hash = sha256Managed.ComputeHash(new UTF8Encoding().GetBytes(text + "EDLight-is beatifull"));
            string hashString = string.Empty;
            foreach (byte i in hash)
            {
                hashString += string.Format("{0:x2}", i);
            }

            return hashString;
        }
        public string EncodeString(string source) => Crypter.EncryptString(password, source);
        public string DecodeString(string source) => Crypter.DecryptString(password, source);
    }
}
