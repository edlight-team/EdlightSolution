using System.Collections.Generic;

namespace SqliteDataExecuter
{
    public static class ListExtension
    {
        public static string AsString<TData>(this List<TData> collection)
        {
            if (collection == null) return "[ 0 , 0 , 0 , 0 , 0 , 0 ]";

            string result = "[ ";

            for (int i = 0; i < collection.Count; i++)
            {
                if (i != collection.Count - 1)
                {
                    result += collection[i].ToString() + " , ";
                }
                else
                {
                    result += collection[i].ToString();
                }
            }

            result += " ]";
            return result;
        }
        public static List<int> AsList(this string source)
        {
            List<int> result = new();

            if (string.IsNullOrEmpty(source)) return result;

            string without_brackets = source.Replace("[ ", string.Empty).Replace(" ]", string.Empty);
            string[] splitted = without_brackets.Split(',');

            foreach (string elem in splitted)
            {
                string trimmed = elem.Trim();
                result.Add(int.Parse(trimmed));
            }

            return result;
        }
    }
}
