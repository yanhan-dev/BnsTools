using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Common
{
    public class Translation
    {
        private static Dictionary<string, string> _Translate;

        public static Dictionary<string, string> Translate
        {
            get { return _Translate ??= LoadTranslate(Config.TranslateFilePath); }
        }


        private static Dictionary<string, string> LoadTranslate(string path)
        {
            Dictionary<string, string> translate = new();

            XDocument xml = XDocument.Load(path);
            foreach (var element in xml.Root.Elements())
            {
                if (element.NodeType == XmlNodeType.Comment)
                {
                    continue;
                }

                string alias = element.Attribute("alias").Value;
                if (!alias.StartsWith("Item.Name2."))
                {
                    continue;
                }

                alias = alias[11..];

                string name = element.Elements().FirstOrDefault().Value;
                translate[alias] = name;
            }
            return translate;
        }
    }
}
