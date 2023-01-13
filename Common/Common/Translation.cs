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

        public static Dictionary<string, string> Translate => _Translate ??= LoadTranslate(Config.TranslatePath);

        private static Dictionary<string, string> _TranslateLower;

        public static Dictionary<string, string> TranslateLower => _TranslateLower ??= TranslateKeyToLower(Translate);

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
                //if (!alias.StartsWith("Item.Name2."))
                //{
                //    continue;
                //}

                //alias = alias[11..];

                string name = element.Elements().LastOrDefault().Value;
                translate[alias] = name;
            }
            return translate;
        }

        //草他妈的NC, text里的有的技能别名大小写混写, 服务端也混写, 没法匹配, 这么大数据量List性能太差, 只能再做个全小写别名的Dictionary了, 
        private static Dictionary<string, string> TranslateKeyToLower(Dictionary<string, string> translate)
        {
            return new Dictionary<string, string>(translate.Select(ss => KeyValuePair.Create(ss.Key.ToLowerInvariant(), ss.Value)).DistinctBy(ss=>ss.Key));
        }
    }
}
