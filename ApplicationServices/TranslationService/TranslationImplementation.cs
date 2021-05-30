using System.Collections.Generic;

namespace ApplicationServices.TranslationService
{
    public class TranslationImplementation : ITranslationService
    {
        private Dictionary<char, string> translate_chars;
        public TranslationImplementation()
        {
            translate_chars = new();

            translate_chars.Add('а', "a");
            translate_chars.Add('б', "b");
            translate_chars.Add('в', "v");
            translate_chars.Add('г', "g");
            translate_chars.Add('д', "d");
            translate_chars.Add('е', "e");
            translate_chars.Add('ё', "yo");
            translate_chars.Add('ж', "j");
            translate_chars.Add('з', "z");
            translate_chars.Add('и', "i");
            translate_chars.Add('й', "i");
            translate_chars.Add('к', "k");
            translate_chars.Add('л', "l");
            translate_chars.Add('м', "m");
            translate_chars.Add('н', "n");
            translate_chars.Add('о', "o");
            translate_chars.Add('п', "p");
            translate_chars.Add('р', "r");
            translate_chars.Add('с', "s");
            translate_chars.Add('т', "t");
            translate_chars.Add('у', "u");
            translate_chars.Add('ф', "f");
            translate_chars.Add('х', "h");
            translate_chars.Add('ц', "c");
            translate_chars.Add('ч', "ch");
            translate_chars.Add('ш', "sh");
            translate_chars.Add('ъ', "");
            translate_chars.Add('ы', "i");
            translate_chars.Add('ь', "");
            translate_chars.Add('э', "e");
            translate_chars.Add('ю', "yu");
            translate_chars.Add('я', "ya");

        }
        public string TranslateWord(string word)
        {
            string result = string.Empty;
            foreach (char ch in word.ToLower())
            {
                result += translate_chars[ch];
            }
            return result;
        }
    }
}
