using Common.Action;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class FileSchemeDescModel
    {
        public string Type { get; set; }
        public string TitleAttr { get; set; }
        public List<string> DescAttr { get; set; }
        public List<AttrDesc> AttrDesc { get; set; }
    }

    public class AttrDesc
    {
        public Dictionary<string, string> Attrs { get; set; }
        public List<TextDesc> TextDesc { get; set; }
        public Dictionary<string, string> LocalDesc { get; set; }
    }

    public class Range
    {
        public int Start { get; set; }
        public int End { get; set; }
    }

    [JsonConverter(typeof(ActionConverter))]
    public class TextDesc
    {
        public string Action { get; set; }
        public IParams Params { get; set; }
    }
}
