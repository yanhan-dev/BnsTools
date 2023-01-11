using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Action
{
    public class DeleteAction
    {
        public const string ACTION = "Delete";
        public const string START = "Start";
        public const string END = "End";

        public static string Do(string value, string start, string end)
        {
            return value.TrimStart(start.ToCharArray()).TrimEnd(end.ToCharArray());
        }
    }
}
