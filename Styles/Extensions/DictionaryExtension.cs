using System;
using System.Collections.Generic;
using System.Linq;

namespace Styles.Extensions
{
    public static class DictionaryExtension
    {
        public static DayOfWeek GetNext(this Dictionary<DayOfWeek, int> dictionary, int offset)
        {
            int index = 0;
            foreach (var key in dictionary.Keys)
            {
                if(index == offset)
                {
                    return key;
                }
                index++;
            }
            return dictionary.FirstOrDefault().Key;
        }
    }
}
