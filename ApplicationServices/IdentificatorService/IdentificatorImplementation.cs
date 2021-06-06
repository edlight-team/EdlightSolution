using System;

namespace ApplicationServices.IdentificatorService
{
    public class IdentificatorImplementation : IIdentificatorService
    {
        private static readonly char[] EngChars = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
        private static readonly char[] NumChars = "0123456789".ToCharArray();
        private static int IDLength;
        private static Random rnd;
        public IdentificatorImplementation()
        {
            IDLength = 8;
            rnd = new();
        }
        public void SetIDLength(int length) => IDLength = length;
        public string GetStringID()
        {
            string ID = string.Empty;
            for (int i = 0; i < IDLength; i++)
            {
                bool isChar = rnd.NextDouble() > 0.5;
                ID += isChar ? EngChars[rnd.Next(0, EngChars.Length)] : NumChars[rnd.Next(0, NumChars.Length)];
            }
            return ID;
        }
        public int GetIntID()
        {
            string ID = string.Empty;
            for (int i = 0; i < IDLength; i++)
            {
                ID += NumChars[rnd.Next(0, NumChars.Length)];
            }
            return Convert.ToInt32(ID);
        }
    }
}
