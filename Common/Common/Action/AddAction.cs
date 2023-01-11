using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Action
{
    public class AddAction
    {
        public const string ACTION = "Add";
        public const string START = "Start";
        public const string END = "End";

        public static string Do(string value, string start, string end)
        {
            return $"{start}{value}{end}";
        }
    }
}
