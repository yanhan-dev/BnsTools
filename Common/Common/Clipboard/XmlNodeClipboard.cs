using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Clipboard
{
    public static class XmlNodeClipboard
    {
        private static Dictionary<string, Dictionary<string, string>> Clipboard { get; set; } = new();
        public static void Copy(Dictionary<string, Dictionary<string, string>> nodes)
        {
            Clipboard = nodes;
        }

        public static Dictionary<string, Dictionary<string, string>> Paste()
        {
            return Clipboard;
        }
    }
}
