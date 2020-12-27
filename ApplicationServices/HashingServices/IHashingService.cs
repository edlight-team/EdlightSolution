using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices.HashingServices
{
    public interface IHashingService
    {
        string GetHash(string text);
    }
}
