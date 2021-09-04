using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExtractItem.POJO.VO
{
    [XmlType(TypeName = "item")]
    public class ItemVO
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("alias")]
        public string Alias { get; set; }
    }
}
