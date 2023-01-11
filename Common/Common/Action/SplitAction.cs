using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Action
{
    public class SplitAction
    {
        public const string ACTION = "Split";
        public const string CHAR = "Char";
        public const string START = "Start";
        public const string END = "End";

        public static string Do(string value, string splitChar, int start, int end)
        {
            if (string.IsNullOrEmpty(splitChar) || start > end)
            {
                throw new ArgumentException("Action: Split, Params: Start > End");
            }
            string[] strings = value.Split(splitChar);
            StringBuilder sb = new();
            for (int i = start; i < end; i++)
            {
                sb.Append(strings[i]);
            }
            return sb.ToString();
        }
    }
}
