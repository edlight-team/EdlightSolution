using System.Security.Cryptography;
using System.Text;

namespace ApplicationServices.HashingServices
{
    public class HashingSHA256Service : IHashingService
    {
        public string GetHash(string text)
        {
            var sha256Managed = new SHA256Managed();
            byte[] hash = sha256Managed.ComputeHash(new UTF8Encoding().GetBytes("заебумба" + text));
            string hashString = string.Empty;
            foreach (var i in hash)
                hashString += string.Format("{0:x2}", i);
            return hashString;
        }
    }
}
