using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Common.Clipboard
{
    public static class XmlAttributeClipboard
    {
        private static Dictionary<string, string> Clipboard { get; set; } = new ();
        public static void Copy(Dictionary<string, string> attrs)
        {
            Clipboard = attrs;
        }

        public static Dictionary<string, string> Paste()
        {
            return Clipboard;
        }
    }
}
